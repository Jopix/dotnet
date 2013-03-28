<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SingUp.aspx.cs" Inherits="view_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>   
    <title>Sing Up</title> 
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>

    <link href="../src/css/SingUpStyle.css" rel="stylesheet" />
    <link href="../src/css/bootstrap-responsive.css" rel="stylesheet" />
    <link href="../src/css/bootstrap.css" rel="stylesheet" />
    <link href="../src/css/TopMenuStyle.css" rel="stylesheet" />

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->

    <style>

        #ismg {
            float:left;
            margin-top:100px;
            margin-left:350px;
        }

    </style>
  </head>

  <body>

   <div class="row-fluid">
       <div class="span12" style="height:auto">
           <!--#include file="../view/topMenu.inc"-->
       </div>
       <div style="height:400px">
           <div class="span4">
               <img id="ismg" src="../res/koam.png" />
           </div>
           <div class="span8">  
               <div class="container" style ="float:right">
              <form class="form-signin">
                <h3 class="form-signin-heading">Please sign in</h3>
                <input type="text" id="userName" class="input-block-level" placeholder="Email address / username" onblur="checkName()">
                  <span id="uN" class="helpinline">  </span>
                <input type="password" id="passWord" class="input-block-level" placeholder="Password" onblur="checkPassWord()">
                  <span id="pw" class ="helpinline">  </span>
                <label class="checkbox">
                  <input type="checkbox" value="remember-me"> Remember me
                </label>
                <button class="btn btn-large btn-primary" type="submit">Sign in</button>
              </form>

            </div> <!-- /container -->
           </div>
       </div>
   </div>
      <div style="width:300px; margin:auto">
                <!-- FOOTER -->
      <footer>
        <p>&copy; 2013 Jopix, Chen Xia. &middot; <a href="#">Privacy</a> &middot; <a href="#">Terms</a></p>
      </footer>

      </div>

      <script src="../src/js/bootstrap.js"></script>
      <script src="../src/js/jquery.js"></script>
      <script src="../src/js/holder.js"></script>
      <script type="text/javascript">

          function checkPassWord() {
              var len = document.getElementById('passWord').value.length;
              if (len == 0)
                  document.getElementById('pw').innerHTML = '密码不能为空';
              else if (len < 6)
                  document.getElementById('pw').innerHTML = '密码无效，密码长度应为大于5';
              else
                  document.getElementById('pw').innerHTML = '';
          }

          function checkName() {
              var len = document.getElementById('userName').value.length;
              if (len == 0)
                  document.getElementById('uN').innerHTML = '请输入用户名';
              else if (len > 10)
                  document.getElementById('uN').innerHTML = '用户名长度最大为10';
              else
                  document.getElementById('uN').innerHTML = '';
          }
      </script>
</body>
</html>
