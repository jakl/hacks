fs = require 'fs'
app = require('https').createServer
  key:fs.readFileSync 'key'
  cert:fs.readFileSync 'cert'
,
(req,res) ->
  console.log req.url
  if req.url is '/'
    fs.readFile 'index.html', (err, data) ->
      res.writeHead 200
      res.end data
  else
    fs.readFile __dirname + req.url, (err, data) ->
      switch req.url[-2..-1]
        when 'js'
          res.writeHead 200, 'Content-Type': 'text/javascript'
        when 'ee'
          res.writeHead 200, 'Content-Type': 'text/javascript'
        when 'ss'
          res.writeHead 200, 'Content-Type': 'text/css'
        else
          res.writeHead 200
      res.end data


app.listen 4240
io = require('socket.io').listen app

io.sockets.on 'connection', (socket) ->
  socket.on 'ls',->
    fs.readdir '.',(err,files)->
      lsfiles = []
      for file in files
        date = fs.statSync(file).mtime
        lsfiles.push name:file,date:date
      socket.emit 'ls',lsfiles
