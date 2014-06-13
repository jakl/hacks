// ==UserScript==
// @name       Cmd+Enter for Jira
// @namespace  Your Jira Domain
// @version    0.1
// @description  When adding a comment allow cmd+enter to submit the comment
// @match      Your Jira Domain/*
// @copyright  2012+, @jakl
// ==/UserScript==

window.onload = function(){
  comment = document.querySelector('#comment')
  if(comment) comment.onkeydown = function(e){
    if(e.keyCode == 13 &amp;&amp; (e.metaKey || e.ctrlKey))
      document.querySelector('#issue-comment-add-submit').click()
  }
}
