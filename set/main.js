var main = {
  up:false,down:false,left:false,right:false,space:false,other:false,
  number:'one',symbol:'diamond',shading:'striped',color:'red',
  title:'The Game of Set',

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
    add_statics(card,{
        red_stripe:this.g.createPattern(get_image('red_stripe.png'),'repeat'),
        green_stripe:this.g.createPattern(get_image('green_stripe.png'),'repeat'),
        purple_stripe:this.g.createPattern(get_image('purple_stripe.png'),'repeat'),
    });

    cards.init();
    for(var i = 0; i < 12; i++)
      cards.draw_card();

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

    var c;
    c = new card({number:this.number,symbol:this.symbol,shading:this.shading,color:this.color,x:0,y:0});
    c.draw(g);
    c = new card({number:1,symbol:1,shading:1,color:1,x:.3,y:.3});
    c.draw(g);
    c = new card({number:2,symbol:2,shading:'solid',color:2,x:.6,y:.6});
    c.draw(g);

    this.draw_controls();
  },
  draw_controls:function(){
    this.g.fillStyle = '#ccc';
    this.draw_control_y = 40;
    this.draw_control(this.title);
    this.draw_control('number   one, two, three');
    this.draw_control('symbol   diamond, squiggle, oval');
    this.draw_control('shading  solid, striped, open');
    this.draw_control('color    red, green, purple');
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
