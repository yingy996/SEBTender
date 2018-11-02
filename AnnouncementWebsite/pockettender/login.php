<?php 
    include("header.php"); 
    include("process_userLogin.php");
?>
<!DOCTYPE html>
<html data-ng-app="">
<head>
    <title>Pocket Tender</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initialscale=1.0"/>
    <!-- Bootstrap -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/stylesheet.css" rel="stylesheet" />
    
</head>
<body background="../images/paintimg.png"> <!--Photo by rawpixel.com from Pexels -->
    <div class="container-fluid" id="contentDiv">
        <div class="row">
            <div class="col-xs-12 col-sm-4 col-sm-offset-4"  style="background-color:rgba(255, 255, 255, 0.8);border-radius:9px;">
                <br/>
                <form id="login" method="post">
                    <fieldset>
                        <legend>Welcome</legend>
                        <p>Login to your account now.</p>
                        
                        <p id="isSuccess" class="text-success"><?php echo $resultMsg; ?></p>
                        <p class="text-danger"><?php echo $errorMsg; ?></p>
                        <div class="form-group">
                            <label for="username">Username:</label>
                            <input type="text" class="form-control" id="username" name="username" placeholder="Enter username"/>
                        </div>

                        <div class="form-group">
                            <label for="password">Password:</label>
                            <input type="password" class="form-control" id="password" name="password"/>
                        </div>
                        <p><input type="submit" class="btn btn-default" id="loginBtn" value="Login"/></p>

                        
                    </fieldset>
                    
                </form>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 text-center">
                <hr/>
                <p>&copy; Developed by Team <em>Dinosaur</em> | Swinburne University of Technology Sarawak</p>
            </div>
        </div>
    </div>
    
    
    
    <!-- jQuery – required for Bootstrap's JavaScript plugins) -->
    <script src="../js/jquery.min.js"></script>
    <!-- All Bootstrap plug-ins file -->
    <script src="../js/bootstrap.min.js"></script>
    <!--Basic AngularJS-->
    
</body>
</html>