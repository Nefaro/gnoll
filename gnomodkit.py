#!/usr/bin/env python3

print('gnomodkit v1 -- https://github.com/minexew/gnomodkit')
print()

'''

# Prerequisites:

  - Gnomoria v1.0
  - Python 3.2+
  - .NET SDK 4.0 or newer

# Usage examples

```
./gnomodkit.py sdk
```

```
./gnomodkit.py mod:ExampleHelloWorld
```

```
./gnomodkit.py sdk mod:ExampleHelloWorld run
```

'''

import argparse
import hashlib
import json
import os
import shutil
import subprocess
import sys
import urllib.request

BUILD_DIR = 'build'
CACHE_DIR = 'cache'
SDK_DIR = 'sdk'

CONFIG_FILENAME = '.config.json'

DOTNETFX_DEFAULT_DIR = 'C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319'

PATCH_BINARY_URL = 'http://gnuwin32.sourceforge.net/downlinks/patch-bin-zip.php'
DE4DOT_URL = 'https://ci.appveyor.com/api/buildjobs/inku0l04uplh1d1r/artifacts/de4dot.zip'
GNOMORIA_HASH = '016e623994628aba2a3cb8cd4cfe2412'

# for SDK
DEOBFUSCATED_FILENAME = os.path.join(BUILD_DIR, 'GnomoriaGame.exe')
DISASSEMBLED_FILENAME = os.path.join(BUILD_DIR, 'GnomoriaGame.il')

SDK_IL_FILENAME = os.path.join(BUILD_DIR, 'GnomoriaSDK.il')
SDK_DLL_FILENAME = os.path.join(SDK_DIR, 'GnomoriaSDK.dll')

# for ModLoader
WORKING_FILENAME = os.path.join(BUILD_DIR, 'GnoMod.il')
OUTPUT_EXE_FILENAME = 'GnoMod.exe'

MOD_LOADER_PATCH = 'ModLoader.patch'

os.makedirs(BUILD_DIR, exist_ok=True)
os.makedirs(CACHE_DIR, exist_ok=True)
os.makedirs(SDK_DIR, exist_ok=True)

parser = argparse.ArgumentParser(description='gnomodkit')
parser.add_argument('-f', dest='force_rebuild', action='store_true',
                   help='force rebuild')
parser.add_argument('targets', nargs=argparse.REMAINDER,
                   help='targets (clean, sdk, mod, run)')
args = parser.parse_args()

config = None

def check_call(args, *other_args, **kwargs):
    print(' '.join(args))
    return subprocess.check_call(args, *other_args, **kwargs)

def get_dotnetfx_dir():
    return config.get('dotnetfx_dir', 'Path to .NET Framework 4? (e.g. ' + DOTNETFX_DEFAULT_DIR + ')')

def get_game_dir():
    return config.get('game_dir', 'Path to game directory? (e.g. C:\\Gnomoria)')

def get_game_mod_dir():
    game_dir = get_game_dir()
    mod_dir = os.path.join(game_dir, "ModLoader")
    os.makedirs(mod_dir, exist_ok=True)
    return mod_dir

def get_ilasm_path():
    return os.path.join(get_dotnetfx_dir(), 'ilasm.exe')

def get_ildasm_path():
    dir = config.get('ildasm_dir', 'Path to ILDASM.exe? (e.g. C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v8.0A\\bin\\NETFX 4.0 Tools)')
    return os.path.join(dir, 'ildasm.exe')

def get_msbuild_path():
    return os.path.join(get_dotnetfx_dir(), 'MSBuild.exe')

def is_up_to_date(product, source):
    try:
        a_mt = os.path.getmtime(product)
        b_mt = os.path.getmtime(source)

        return a_mt > b_mt
    except FileNotFoundError:
        return False

def md5sum(filename):
    with open(filename, mode='rb') as f:
        d = hashlib.md5()
        while True:
            buf = f.read(4096)
            if not buf:
                break
            d.update(buf)
        return d.hexdigest()

def platform_is_windows():
    return sys.platform.startswith('win')

def save_config():
    pass

class Config:
    def __init__(self):
        if os.path.exists(CONFIG_FILENAME):
            with open(CONFIG_FILENAME) as conf:
                self.config = json.load(conf)
        else:
            self.config = {}

    def get(self, key, prompt=None):
        if not key in self.config and prompt:
            print(prompt)
            print('>', end=' ')
            self.set(key, input())

        return self.config[key]

    def isset(self, key):
        return key in self.config

    def save(self):
        with open(CONFIG_FILENAME, 'w') as conf:
            json.dump(self.config, conf)

    def set(self, key, value):
        self.config[key] = value
        self.save()


class Task:
    def __init__(self):
        self.dependencies = []

        self.ignore_outdated_dependencies = True

    def add_dependency(self, task):
        self.dependencies.append(task)

    def build_dependency_tree(self):
        self.discover_dependencies()

        for dep in self.dependencies:
            dep.build_dependency_tree()

    def discover_dependencies(self):
        pass

    def is_up_to_date(self):
        return False

    def needs_to_rerun(self):
        if not self.ignore_outdated_dependencies:
            for dep in self.dependencies:
                if dep.needs_to_rerun():
                    return True

        return not self.is_up_to_date()

    def run(self):
        pass

    def run_dependencies(self, force, made_dependencies = {}, indent = 0):
        for dep in self.dependencies:
            if force or dep.needs_to_rerun():
                print('  ' * indent, end='')

                dep_desc = str(dep)

                if dep_desc in made_dependencies:
                    print('-- skipping `%s`, already done' % (dep_desc))
                    continue
                else:
                    if isinstance(self, TaskRunner):
                        print('-- %s' % dep_desc)
                    else:
                        print('-- %s\t(dependency for: %s)' % (dep_desc, self))

                    dep.run_dependencies(force, made_dependencies, indent + 1)
                    dep.run()

                    made_dependencies[dep_desc] = True

class TaskApplyPatch(Task):
    def __init__(self, filename, output_filename, patch_filename):
        super().__init__()
        self.filename = filename
        self.output_filename = output_filename
        self.patch_filename = patch_filename

    def __str__(self):
        return 'apply ' + self.patch_filename

    def discover_dependencies(self):
        self.patch = TaskGetPatchBinary()
        self.add_dependency(self.patch)

    def is_up_to_date(self):
        return is_up_to_date(self.output_filename, self.filename) and is_up_to_date(self.output_filename, self.patch_filename)

    def run(self):
        check_call([self.patch.patch_path, '-l', '-o', self.output_filename, self.filename, self.patch_filename])

class TaskAssemble(Task):
    def __init__(self, filename, output_filename):
        super().__init__()
        self.filename = filename
        self.output_filename = output_filename

        self.ilasm_path = get_ilasm_path()

    def __str__(self):
        return 'assemble ' + self.output_filename

    def is_up_to_date(self):
        return is_up_to_date(self.output_filename, self.filename)

    def run(self):
        with open(self.output_filename + '.log', 'wt') as log:
            check_call([self.ilasm_path, '/out=' + self.output_filename, self.filename], stdout=log)

class TaskCheckDistFile(Task):
    def __init__(self, path, required_hash):
        super().__init__()

        self.relative_path = path
        self.path = os.path.join(get_game_dir(), path)
        self.required_hash = required_hash

    def __str__(self):
        return 'check %s' % self.relative_path

    def run(self):
        file_hash = md5sum(self.path)
        if file_hash != self.required_hash:
            raise Exception('ERROR: Hash of "%s" is %s instead of expected %s' % (
                    self.relative_path, file_hash, self.required_hash))

class TaskClean(Task):
    def __str__(self):
        return 'clean'

    def run(self):
        shutil.rmtree(BUILD_DIR, True)
        shutil.rmtree(SDK_DIR, True)

class TaskCopyFile(Task):
    def __init__(self, filename, output_filename):
        super().__init__()
        self.filename = filename
        self.output_filename = output_filename

    def __str__(self):
        return 'copy ' + self.output_filename

    def is_up_to_date(self):
        return is_up_to_date(self.output_filename, self.filename)

    def run(self):
        shutil.copyfile(self.filename, self.output_filename)

class TaskDecompile(Task):
    def __init__(self, filename, output_filename):
        super().__init__()
        self.filename = filename
        self.output_filename = output_filename

        self.ildasm_path = get_ildasm_path()

    def __str__(self):
        return 'decompile ' + self.filename

    def is_up_to_date(self):
        return is_up_to_date(self.output_filename, self.filename)

    def run(self):
        check_call([self.ildasm_path, '/out=' + self.output_filename, self.filename])

class TaskDeobfuscate(Task):
    def __init__(self, filename, output_filename):
        super().__init__()
        self.filename = filename
        self.output_filename = output_filename

    def __str__(self):
        return 'deobfuscate %s' % self.filename

    def discover_dependencies(self):
        self.de4dot = TaskGetDe4dot()

        self.add_dependency(self.de4dot)

    def is_up_to_date(self):
        # Not quite correct. Fixable?
        return os.path.exists(self.output_filename)

    def run(self):
        with open(os.path.join(BUILD_DIR, 'deobfuscate.log'), 'wt') as log:
            check_call([self.de4dot.de4dot_path,
                    '--keep-names', 'n', '-v', self.filename, '-o', self.output_filename], stdout=log)

class TaskDownloadFile(Task):
    def __init__(self, url, save_as):
        super().__init__()
        self.url = url
        self.save_as = save_as

    def __str__(self):
        return 'download %s' % self.url

    def run(self):
        #urllib.request.urlretrieve(self.url, self.save_as)

        # http://stackoverflow.com/a/27971337/2524350
        f = open(self.save_as, 'wb')
        remote_file = urllib.request.urlopen(self.url)

        try:
            total_size = remote_file.info()['Content-Length'].strip()
            header = True
        except AttributeError:
            header = False # a response doesn't always include the "Content-Length" header

        if header:
            total_size = int(total_size)

        bytes_so_far = 0

        while True:
            buffer = remote_file.read(8192)
            if not buffer:
                sys.stdout.write('\n')
                break

            bytes_so_far += len(buffer)
            f.write(buffer)
            if not header:
                total_size = bytes_so_far # unknown size

            percent = float(bytes_so_far) / total_size
            percent = round(percent*100, 2)
            sys.stdout.write("Downloaded %d of %d bytes (%0.2f%%)\r" % (bytes_so_far, total_size, percent))
            sys.stdout.flush()

class TaskExtractZip(Task):
    def __init__(self, filename, output_dir):
        super().__init__()
        self.filename = filename
        self.output_dir = output_dir

    def __str__(self):
        return 'extract %s' % self.filename

    def run(self):
        if os.path.exists(self.output_dir):
            shutil.rmtree(self.output_dir)

        os.makedirs(self.output_dir)

        import zipfile
        zip_ref = zipfile.ZipFile(self.filename, 'r')
        zip_ref.extractall(self.output_dir)
        zip_ref.close()

class TaskGetDe4dot(Task):
    def __init__(self):
        super().__init__()

        self.de4dot_dir = os.path.join(CACHE_DIR, 'de4dot')
        self.de4dot_path = os.path.join(self.de4dot_dir, 'de4dot.exe')
        self.ignore_outdated_dependencies = True

    def __str__(self):
        return 'get de4dot'

    def discover_dependencies(self):
        zipfile_path = os.path.join(CACHE_DIR, 'de4dot.zip')

        self.add_dependency(TaskDownloadFile(DE4DOT_URL, zipfile_path))
        self.add_dependency(TaskExtractZip(zipfile_path, self.de4dot_dir))

    def is_up_to_date(self):
        # Not quite correct. Fixable?
        return os.path.exists(self.de4dot_dir)

class TaskGetPatchBinary(Task):
    def __init__(self):
        super().__init__()

        self.patch_dir = os.path.join(CACHE_DIR, 'patch')
        self.patch_path_orig = os.path.join(self.patch_dir, 'bin/patch.exe')
        self.patch_path = os.path.join(self.patch_dir, 'bin/apply_changeset.exe')
        self.ignore_outdated_dependencies = True

    def __str__(self):
        return 'get patch.exe'

    def discover_dependencies(self):
        zipfile_path = os.path.join(CACHE_DIR, 'patch.zip')

        self.add_dependency(TaskDownloadFile(PATCH_BINARY_URL, zipfile_path))
        self.add_dependency(TaskExtractZip(zipfile_path, self.patch_dir))

    def is_up_to_date(self):
        # Not quite correct. Fixable?
        return os.path.exists(self.patch_dir)

    def run(self):
        # UAC is stupid
        os.rename(self.patch_path_orig, self.patch_path)

class TaskMakeAllFieldsPublic(Task):
    def __init__(self, filename, output_filename):
        super().__init__()
        self.filename = filename
        self.output_filename = output_filename

    def __str__(self):
        return 'make-all-fields-public ' + self.filename

    def is_up_to_date(self):
        return is_up_to_date(self.output_filename, self.filename)

    def run(self):
        with open(self.filename) as input, open(self.output_filename, 'w') as output:
            for line in input:
                line = line.replace('.field private', '.field public')
                line = line.replace('.method private', '.method public')
                print(line, end='', file=output)

class TaskMakeMod(Task):
    def __init__(self, solution_dir):
        super().__init__()
        self.name = os.path.basename(os.path.normpath(solution_dir))
        self.solution_dir = solution_dir
        self.project_dir = os.path.join(self.solution_dir, self.name)
        self.dll_name = self.name + ".dll"

    def __str__(self):
        return 'make ' + self.name

    def discover_dependencies(self):
        # make SDK
        self.add_dependency(TaskMakeSDK())

        # build mod
        self.add_dependency(TaskMsbuild(os.path.join(self.solution_dir, self.name + ".sln")))

        # install into game directory
        self.add_dependency(TaskCopyFile(os.path.join(self.project_dir, 'bin\\Debug', self.dll_name), os.path.join(get_game_mod_dir(), self.dll_name)))

class TaskMakeModLoader(Task):
    def __str__(self):
        return 'make ModLoader'

    def discover_dependencies(self):
        solution_dir = 'ModLoader'
        project_dir = os.path.join(solution_dir, 'ModLoader')
        modloader_dll = 'ModLoader.dll'

        # make SDK
        self.add_dependency(TaskMakeSDK())

        # build ModLoader
        self.add_dependency(TaskMsbuild(os.path.join(solution_dir, 'ModLoader.sln')))
        self.add_dependency(TaskCopyFile(os.path.join(project_dir, 'bin\\x86\\Debug', modloader_dll), os.path.join(get_game_dir(), modloader_dll)))

        # patch ModLoader hooks into GnomoriaSDK
        self.add_dependency(TaskApplyPatch(SDK_IL_FILENAME, WORKING_FILENAME, MOD_LOADER_PATCH))

        # assemble GnoMod
        self.add_dependency(TaskAssemble(WORKING_FILENAME, os.path.join(get_game_dir(), OUTPUT_EXE_FILENAME)))

class TaskMakeSDK(Task):
    def __str__(self):
        return 'make SDK'

    def discover_dependencies(self):
        # find and check Gnomoria.exe
        self.exe = TaskCheckDistFile('Gnomoria.exe', GNOMORIA_HASH)
        self.add_dependency(self.exe)

        # undo obfuscator damage
        self.add_dependency(TaskDeobfuscate(self.exe.path, DEOBFUSCATED_FILENAME))

        # disassemble into IL code
        self.add_dependency(TaskDecompile(DEOBFUSCATED_FILENAME, DISASSEMBLED_FILENAME))

        # make all fields & methods public
        self.add_dependency(TaskMakeAllFieldsPublic(DISASSEMBLED_FILENAME, SDK_IL_FILENAME))

        # build GnomoriaSDL.dll
        self.add_dependency(TaskAssemble(SDK_IL_FILENAME, SDK_DLL_FILENAME))

    def is_up_to_date(self):
        return is_up_to_date(SDK_DLL_FILENAME, self.exe.path)

class TaskMsbuild(Task):
    def __init__(self, solution):
        super().__init__()
        self.solution = solution

        self.msbuild_path = get_msbuild_path()

    def __str__(self):
        return 'msbuild %s' % self.solution

    def run(self):
        check_call([self.msbuild_path, self.solution])

class TaskRunModdedGame(Task):
    def __str__(self):
        return 'run'

    def discover_dependencies(self):
        self.add_dependency(TaskMakeModLoader())

    def run(self):
        exe = os.path.join(get_game_dir(), OUTPUT_EXE_FILENAME)
        dir = get_game_dir()
        subprocess.run(exe, cwd=dir)

class TaskRunner(Task):
    pass

config = Config()

if not config.isset('dotnetfx_dir') and os.path.exists(DOTNETFX_DEFAULT_DIR):
    config.set('dotnetfx_dir', DOTNETFX_DEFAULT_DIR)

tr = TaskRunner()

targets = args.targets if len(args.targets) else args.targets

for target in targets:
    if target == 'clean':
        tr.add_dependency(TaskClean())
    elif target.startswith('mod:'):
        tr.add_dependency(TaskMakeMod(target[4:]))
    elif target == 'modloader':
        tr.add_dependency(TaskMakeModLoader())
    elif target == 'run':
        tr.add_dependency(TaskRunModdedGame())
    elif target == 'sdk':
        tr.add_dependency(TaskMakeSDK())
    else:
        raise Exception('invalid target: ' + target)

tr.build_dependency_tree()
tr.run_dependencies(args.force_rebuild)
