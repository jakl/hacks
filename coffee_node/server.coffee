fs = require 'fs'
app = require('https').createServer
  key:fs.readFileSync '/etc/ssl/private/server.key'
  cert:fs.readFileSync '/etc/ssl/certs/server.crt'
,
(req,res) ->
  fs.readFile __dirname + req.url, (err, data) ->
    if 'js' is req.url[-2..-1]
      res.writeHead 200, 'Content-Type': 'text/javascript'
    else
      res.writeHead 200
    res.end data

app.listen 443
io = require('socket.io').listen app

io.sockets.on 'connection', (socket) ->
  socket.emit 'news', hello: 'world'
  socket.on 'my other event', (data) ->
    console.log data
