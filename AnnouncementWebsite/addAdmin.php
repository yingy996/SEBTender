<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>SESCO eTender Announcements</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- Bootstrap -->
        <link href="css/bootstrap.min.css" rel="stylesheet" />
        <link href="css/stylesheet.css" rel="stylesheet" />

    </head>
    <body id="loginpg">
        <?php 
        include("header.php");
        
        include("process_addAdmin.php");
        ?>
        
        <!-- Body -->
        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-4 col-xs-offset-4">
                    <form id="addAdmin" name="addAdmin" method="post" action="" novalidate role="form">
                        <fieldset>
                            <legend>New Admin Registration</legend>
                            <?php if(!empty($success_message)) { ?>	                           
                                <div class="alert alert-success"><?php if(isset($success_message)) echo $success_message; ?></div>                   
                            <?php } ?>
                            
                            <?php if(!empty($error_message)) { ?>	                            
                                <div class="alert alert-danger"><?php if(isset($error_message)) echo $error_message; ?></div>
                            <?php } ?>
                            
                            <div class="form-group">
                                <label for="name">Name:</label>
                                <input type="text" class="form-control" id="name" name="name" placeholder="Enter name" value="<?php if(isset($_POST["name"])) echo $_POST["name"]; ?>" required/>
                                <span class="error"><?php if($nameError != "") echo "<p class='alert alert-danger'>" . $nameError . "</p>";?></span>
                            </div>
                            
                            <div class="form-group">
                                <label for="email">Email:</label>
                                <input type="email" class="form-control" id="email" name="email" placeholder="Enter email" value="<?php if(isset($_POST["email"])) echo $_POST["email"]; ?>" required/>
                                <span class="error"><?php if($emailError != "") echo "<p class='alert alert-danger'>" . $emailError . "</p>";?></span>
                            </div>
                            
                            <div class="form-group">
                                <label for="role">Role:</label>
                                <select class="form-control" name="role" required>
                                    <option value="admin">Administrator</option>
                                    <option value="editor">Editor</option>
                                </select>
                            </div>
                            
                            
                            <div class="form-group">
                                <label for="username">Username:</label>
                                <input type="text" class="form-control" id="username" name="username" placeholder="Enter username" value="<?php if(isset($_POST["username"])) echo $_POST["username"]; ?>" required/>
                                <span class="error"><?php if($usernameError != "") echo "<p class='alert alert-danger'>" . $usernameError . "</p>";?></span>
                            </div>

                            <div class="form-group">
                                <label for="password">Password:</label>
                                <input type="password" class="form-control" id="password" name="password" required/>
                                <span class="error"><?php if($passwordError != "") echo "<p class='alert alert-danger'>" . $passwordError . "</p>";?></span>
                            </div>
                            
                            <div class="form-group">
                                <label for="password">Confirm Password:</label>
                                <input type="password" class="form-control" id="confPassword" name="confPassword" required/>
                                <span class="error"><?php if($confPasswordError != "") echo "<p class='alert alert-danger'>" . $confPasswordError . "</p>";?></span>
                            </div>
                            <p><input type="submit" name="registerAdminBtn" class="btn btn-default" id="registerAdminBtn" value="Register"/></p>
                        </fieldset>

                    </form>
                </div>
            </div>
        </div>
        
        
        
        <!-- Footer -->
        <?php 
        include("footer.php");
        ?>
        
        <!-- jQuery – required for Bootstrap's JavaScript plugins) -->
        <script src="js/jquery.min.js"></script>
        <!-- All Bootstrap plug-ins file -->
        <script src="js/bootstrap.min.js"></script>
        <!--Basic AngularJS-->
    </body>
</html>