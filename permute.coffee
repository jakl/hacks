#!/usr/bin/env coffee
run=->
    input = process.argv[2]

    unless input
        console.log 'coffee ' + process.argv[1] + " 'MyMostAwesome[\\w\\d]{2,4}Today'"
        process.exit 0

    recursive input

test=->
    console.log 'Trying d'
    console.log charsInClass 'd'
    console.log 'Trying s'
    console.log charsInClass 's'
    console.log 'Trying [[-`]'
    console.log charsInRange '[', '`'
    console.log 'Trying [\\d]{0,1}'
    console.log stringsOfCharsInQuantifiers charsInClass('d'), 0,4

recursive=(rest, prefix = '')->
    #Gather prefix e.g. prefix(options)
    while rest isnt '' and rest[0] isnt '['
        prefix += rest[0]
        rest = rest[1..]

    #Print and end when we are done
    unless rest
        console.log prefix
        return

    #Gather options and delete them from the string
    rest = rest[1..]
    close = rest.indexOf ')'
    options = rest[...close].split ','
    rest = rest[close+1..]

    #Recurse on newly found permutations
    for option in options
        recursive rest, prefix+option

charsInClass=(abbreviation)->
    ascii=String.fromCharCode
    switch abbreviation
        when 'd' then ascii x for x in [48..57]
        when 's' then ['\n','\t',' ']
        when 't' then ['\t']
        when 'n' then ['\n']
        when 'w' then ['_'].concat (ascii x for x in [48..57]), (ascii x for x in [65..90]), (ascii x for x in [97..122])
        else []

charsInRange=(left,right)-> String.fromCharCode x for x in [left.charCodeAt(0)..right.charCodeAt(0)]

stringsOfCharsInQuantifiers=(chars,min,max)->
    stringsOfCharsInQuantifier=(rest,prefix=[''])->
        return prefix unless rest
        for char in chars
            stringsOfCharsInQuantifier rest-1, prefix+char

    strings = stringsOfCharsInQuantifier min

    for i in [0...max-min]
        for j in [0...strings.length]
            strings.push strings[j]+char for char in chars

    strings

test()
