This mod fixes a game crash caused by a dying resource pile.

The story is that a gnome, wielding a wheelbarrow with a full resource pile, is fighting a golem. The gnome dies, and the game tries to throw the resources inside the pile onto the ground, essentially "killing" the pile during the process.

The functionality that processes death events expects no deaths during processing. As such, the resource pile dying is unexpected, and the game crashes.