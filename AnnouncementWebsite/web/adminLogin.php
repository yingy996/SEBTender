<?php 
    include("header.php");
    include("process_adminLogin.php");
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
<body id="loginpg"> <!--full page background img -->

    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-4 col-xs-offset-4">
                <br/>
                <form id="login" method="post">
                    <fieldset>
                        <legend>Welcome Administrator</legend>
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