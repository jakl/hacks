var zones = {
  load : 
    function (filename){
      var script=document.createElement('script');
      script.type = "text/javascript";
      script.src = "zones/"+filename;
      document.getElementsByTagName("head")[0].appendChild(script);
    },
  get_sprite_name :
  function (zone, x, y){
    return this[zone].tiles.y.substr(x-1,1)
  mapping :
  {
    'g' :
    {
      name : 'grass',
      can_walk : true,
//    sprite_name : 'grass2',
    },
    ' ' :
    {
      name : 'blank',
    },
  },
}

zones.load('a.js');
