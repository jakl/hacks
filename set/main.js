var main = {
  up:false,down:false,left:false,right:false,space:false,other:false,
  dat_message:'dawg',

  tic:function(){
    this.paint();
    setTimeout("main.tic()", 100);
  },
  resize:function(){
    main.g.canvas.width = window.innerWidth*.95;
    main.g.canvas.height = window.innerHeight*.95;
    main.paint();
  },
  init:function(){
    this.g = document.getElementById('canvas').getContext('2d');

    //document.onmouseup = function(e){}
    //document.onmousedown = function(e){}
    //document.onmousemove = function(e){}
    //document.onmousewheel = function (e){}
    window.onresize=this.resize;

    this.resize();
  },
  paint:function(){
    var g = this.g;
    g.fillStyle = '#000';
    g.fillRect(0,0,this.g.canvas.width,this.g.canvas.height);

    g.fillStyle = '#315';
    this.draw_diamond(g,0,0,.3,.3);

    this.draw_controls();
  },
  draw_diamond:function(g,x,y,w,h){
    g.beginPath();
    x=g.canvas.width*x;
    y=g.canvas.height*y;
    w=g.canvas.width*w;
    h=g.canvas.height*h;
    g.moveTo(0,h/2);
    g.lineTo(w/2,0);
    g.lineTo(w,h/2);
    g.lineTo(w/2,h);
    g.lineTo(0,h/2);
    g.closePath();
    g.fill();
  },
  draw_controls:function(){
    this.g.fillStyle = '#ccc';
    this.draw_control_y = 40;
    this.draw_control("The Game of Set");
    var c = new card('one','oval','open','green');
    console.log(c);
    console.log(c.symbol);
    console.log(c.symbol.get_third(new symbol('oval')));
    this.draw_control(c.symbol.get_third(new symbol('oval')).string);
  },
  draw_control:function(message){
    this.g.fillText(message,this.g.canvas.width/2,this.draw_control_y+=20);
  },
  key_down:function(e){
      if (e.keyCode == 39) {right=true;}
      else if (e.keyCode == 37) {this.left=true;}
      else if (e.keyCode == 38) {this.up=true;}
      else if (e.keyCode == 40) {this.down=true;}
      else if (e.keyCode == 32) {this.space=true;}
      else {this.other=true;}
      this.paint();
    return true;//false to capture input and decieve browser
  },
  key_up:function(e){
      if (e.keyCode == 39) {this.right=false;}
      else if (e.keyCode == 37) {this.left=false;}
      else if (e.keyCode == 38) {this.up=false;}
      else if (e.keyCode == 40) {this.down=false;}
      else if (e.keyCode == 32) {this.space=false;}
      else {this.other=false;}
      this.paint();
    return true;//false to capture input and decieve browser
  },
}
