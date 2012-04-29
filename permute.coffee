input = process.argv[2]

unless input
    console.log 'coffee ' + process.argv[1] + " 'MyMostAwesome[\w\d]{2,4}Today'"
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

recursive input
