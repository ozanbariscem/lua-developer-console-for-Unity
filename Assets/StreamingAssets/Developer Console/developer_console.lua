-- Check out https://www.moonsharp.org/

-- You can add functions without having to deal with c# or unity
-- And since this is located at StreamingAssets you wouldn't need to build again
-- And your players can modify this file as they want with limitations you set -Through exposing-
-- These are just some basic examples but possibilities are endless
-- You could have GameManager class referencing every other Managers in your game
-- and expose the GameManager to Lua to basically handle the entire game logic

-- I exposed the Log function on DeveloperConsole.cs LoadScript function
function add(...)
    local numbers = {...}

    local total = 0
    local log = ""
    for i = 1, #numbers do
        local num = tonumber(numbers[i])
        if num then
            total = total + num
            log = log..num
            if i != #numbers then
                log = log.." + "
            end
        end
    end

    log = log.."\n= "..total
    Log(log)
    return total
end

function multiply(...)
    local numbers = {...}

    local total = 1
    local log = ""
    for i = 1, #numbers do
        local num = tonumber(numbers[i])
        if num then
            total = total * num
            log = log..num
            if i != #numbers then
                log = log.." * "
            end
        end
    end
    
    log = log.."\n= "..total
    Log(log)
    return total
end

-- I exposed the Random function on DeveloperConsole.cs LoadScript function
function hello()
    local greets = {"Hey.", "Hello!", "Salut", "Merhaba", "Privyet", "Selam", "Whatever"}
    -- Lua arrays start with 1 and Random.max isn't inclusive so we add + 1
    local random_greet = greets[Random(1, #greets + 1)]
    Log(random_greet)
end

function i_love_you()
    Log("Aww, thanks!")
end

-- I exposed the UI class on DeveloperConsole.cs LoadScript function
function clear()
    UI.ClearLog()
end