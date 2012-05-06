#!/usr/bin/env coffee
run=->
    input = process.argv[2]

    unless input
        console.log 'coffee ' + process.argv[1] + " '[x-z]?[pP]a[s5]{1,2}word\\d+'"
        process.exit 0

    generate input

generate=(rest, prefix='')->
    return console.log prefix unless rest

    if rest[0] is '['
        endRange = rest.indexOf ']'
        chars = rest[1...endRange]
        rest = rest[endRange+1..]
    else if rest[0] is '\\' and rest[0..1] of cache
        chars = rest[0..1]
        rest = rest[2..]
    else
        chars = rest[0]
        rest = rest[1..]

    if rest[0] is '{'
        endRange = rest.indexOf '}'
        multiplier = rest[1...endRange]
        rest = rest[endRange+1..]

        [start, stop] = multiplier.split ','
        start = Number start
        stop = start unless stop? #There wasn't a comma, so start and stop on the same length
        stop = undefined if stop is '' #There was a comma, so let stop be the default for permute
        permute chars, rest, prefix, start, stop

    else if rest[0] is '+'
        permute chars, rest[1..], prefix, 1
    else if rest[0] is '*'
        permute chars, rest[1..], prefix
    else if rest[0] is '?'
        generate rest[1..], prefix
        generate rest[1..], prefix+c for c in charsIn chars
    else
        generate rest, prefix+c for c in charsIn chars


#Cycle through each string produced by a char set followed by braces and pass it to generate as a prefix
# [a-c]{0,4}
permute=(chars, rest='', prefix='', start=0, stop=4)->
    if start
        permute chars, rest, prefix+c, start-1, stop-1 for c in charsIn chars
    else if stop
        permute chars, rest, prefix+c, start, stop-1 for c in charsIn chars
        generate rest, prefix
    else
        generate rest, prefix
    return 0

#Cache and return the list of characters that would match a bracket range [a-m] or a backslash character class \d
charsIn=(input)->
    return cache[input] if input of cache
    return [input[0]] if input.length is 1

    cacheKey = input

    chars = []
    while input.length
        if input[0] is '\\' and input[0..1] of cache #if char class or escaped char
            chars = chars.concat cache[input[0..1]]
            input = input[2..] #remove \w from input
        else if input[1] is '-' and input.length > 2 #a range
            chars.push String.fromCharCode x for x in [input[0].charCodeAt(0)..input[2].charCodeAt(0)]
            input = input[3..]
        else #if single char
            chars.push input[0]
            input = input[1..]

    #Uniqify chars
    o = {}
    o[i] = 1 for i in chars
    cache[cacheKey] = Object.keys o

loadCache=->
    ascii = String.fromCharCode
    @cache= #cache character classes \d or ranges [h-z]
        '\\d': (ascii x for x in [48..57])
        '\\s': ['\n','\t',' ']
        '\\t': ['\t']
        '\\n': ['\n']
        '\\w': (['_'].concat (x.toString() for x in [0..9]), (ascii x for x in [65..90]), (ascii x for x in [97..122]))

loadCache()
run()
