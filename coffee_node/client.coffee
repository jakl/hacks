@game.main =
  files:{}
  init:->
    @socket = io.connect 'https://kk.myftp.biz'
    @socket.on 'ls',(files)=>
      @files = files
      for file in @files
        file.date = file.date.replace 'T',' '
        file.date = file.date.replace '.000Z',''
      @clearTable()
      @genTable()
    @socket.emit 'ls'
    @table = $('#files')[0]
  clearTable:->
    while @table.hasChildNodes()
      @table.removeChild @table.firstChild
  genTable:->
    row = @table.insertRow @table.rows.length
    cell = row.insertCell()
    cell.innerHTML = "Date"
    cell.onclick = =>
      @files.sort (a,b)->
        return 1 if a.date > b.date
        return -1 if a.date < b.date
        return 0
      @clearTable()
      @genTable()
    cell = row.insertCell()
    cell.innerHTML = "Name"
    cell.onclick = =>
      @files.sort (a,b)->
        return 1 if a.name > b.name
        return -1 if a.name < b.name
        return 0
      @clearTable()
      @genTable()

    for file in @files
      date = file.date
      name = file.name

      row = @table.insertRow @table.rows.length
      row.insertCell().innerHTML = "<a href=#{name} id=dl>#{name}</a>"
      row.insertCell().innerHTML = "<p id=date>#{date}</a>"
      row.onclick = ->
        color = $(@).css 'backgroundColor'
        if color is "rgb(255, 255, 255)"
          $(@).css backgroundColor:'black'
        else
          $(@).css backgroundColor:'white'
  key_down:(e)->
    switch e.keyCode
      when 39
        @socket.emit 'ls'
        @right=true
      when 37
        @clearTable()
        @genTable()
        @left=true
      when 38
        $('#player')[0].play()
        @up=true
      when 40 then @down=true
      when 32 then @space=true
      else @other=true
    return true #false to capture input and decieve browser
  key_up:(e)->
    switch e.keyCode
      when 39 then @right=false
      when 37 then @left=false
      when 38 then @up=false
      when 40 then @down=false
      when 32 then @space=false
      else @other=false
    return true
