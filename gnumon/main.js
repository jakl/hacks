var h, w, xmouse, ymouse;//Screen height and width, and mouse x and y positions
var g = document.getElementById('canvas').getContext('2d');

var overworld = {width:80,height:80,vieww:20,viewh:10,
                  sprites:{grass:getImage('grass.png'),
                           lava: getImage('lava.png'),
                           water:getImage('water.png'),
                           trainer:getImage('trainer.png'),
                           cave:getImage('cave.png')},
                  init:init_tiles, draw:draw_tiles};

var playerx=2, playery=2;

init();
setTimeout("paint()",10);//wait for images to load

window.onresize=resize;

function resize(){
  h = window.innerHeight*.95;
  w = window.innerWidth*.95;
  g.canvas.width = w;
  g.canvas.height = h;
}
 
function init(){
  resize();
  overworld.init();
}
 
document.onmousemove = function(e){
  xmouse = e.pageX/w;
  ymouse = e.pageY/h;
}
 
document.onmouseup = function(e){}
document.onmousedown = function(e){}
 
function paint(){
  g.fillStyle = '#000';
  g.fillRect(0,0,w,h);
  
  overworld.draw(playerx,playery);
 
  drawControls();
}
 
function drawControls(){
  g.fillStyle = '#ccc';
  controlYPos = 40;
  drawControl("Space - Zoom Out");
  drawControl("Secret - Zoom In");
  drawControl("Arrow Keys - Move Pokemon Trainer");
}

var controlYPos=40;
function drawControl(message){
  g.fillText(message,w/2,controlYPos+=20);
}
 
var up=false,down=false,left=false,right=false,space=false,other=false;
 
function key_down(e) {
    if (e.keyCode == 39) {right=true;playerx++;paint();}
    else if (e.keyCode == 37) {left=true;playerx--;paint();}
    else if (e.keyCode == 38) {up=true;playery--;paint();}
    else if (e.keyCode == 40) {down=true;playery++;paint();}
    else if (e.keyCode == 32) {space=true;overworld.viewh+=2;overworld.vieww+=4;paint();}
    else {other=true;overworld.viewh-=2;overworld.vieww-=4;paint();}
	return true;//false to capture input and decieve browser
}
function key_up(e) {
    if (e.keyCode == 39) {right=false;}
    else if (e.keyCode == 37) {left=false;}
    else if (e.keyCode == 38) {up=false;}
    else if (e.keyCode == 40) {down=false;}
    else if (e.keyCode == 32) {space=false;}
    else {other=false;}
  return true;//false to capture input and decieve browser
}
