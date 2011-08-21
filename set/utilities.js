function add_statics(object, properties){
  for (var property_name in properties)
    object[property_name] = properties[property_name];
}
function add_members(object, properties){
  for (var property_name in properties)
    object.prototype[property_name] = properties[property_name];
}
function get_image(name){
    var img = new Image();
    img.src=name;
    return img;
}
