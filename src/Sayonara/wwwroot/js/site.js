﻿// Write your Javascript code.
$(function () {

    $("div.hoverhighlight").hover(
      function () {
          $(this).addClass("highlighted");
      }, function () {
          $(this).removeClass("highlighted");
      }
    );
    console.log('hello world')
});
