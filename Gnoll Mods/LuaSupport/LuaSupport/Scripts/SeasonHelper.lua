SeasonHelper = {}

local Season = require "Season"
local _GN = _GNOMORIA;

function SeasonHelper.currentSeason()
    for ses, idx in pairs(Season) do
        if ( idx == _GN.getCurrentSeason() ) then
            return ses
        end
    end
end

return SeasonHelper