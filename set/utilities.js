function set_static_properties(object, properties){
    for (property_name in properties)
          object[property_name] = properties[property_name];
}
function set_member_properties(object, properties){
    for (property_name in properties)
          object.prototype[property_name] = properties[property_name];
}
