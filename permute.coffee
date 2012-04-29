recurOrIter = process.argv[2]
input = process.argv[3]

unless recurOrIter
    console.log 'coffee ' + process.argv[1] + " recursive '(Titans,Seahawks) (won,lost,tied) with (1,2,3,4,5,6) points'"
    console.log 'coffee ' + process.argv[1] + " iterative '(Titans,Seahawks) (won,lost,tied) with (1,2,3,4,5,6) points'"
    process.exit 0

recursive=(rest, prefix = '')->
    #Gather prefix e.g. prefix(options)
    while rest isnt '' and rest[0] isnt '('
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

iterative=(rest)->
    q = [''] #A queue to store permutations
    loop
        #Gather prefix e.g. prefix(options)
        prefix = ''
        while rest isnt '' and rest[0] isnt '('
            prefix += rest[0]
            rest = rest[1..]

        #Print and end when we are done
        unless rest
            console.log phrase+prefix for phrase in q
            return

        #Gather options and delete them from the string
        rest = rest[1..]
        close = rest.indexOf ')'
        options = rest[...close].split ','
        rest = rest[close+1..]

        #Store newly found permutations
        for [1..q.length]
            previous = q.shift()
            for option in options
                q.push previous+prefix+option


recursive input if recurOrIter.match /^r/i
iterative input if recurOrIter.match /^i/i
