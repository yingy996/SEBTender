<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>SESCO eTender Announcements</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- Bootstrap -->
        <link href="../css/bootstrap.min.css" rel="stylesheet" />
        <link href="../css/stylesheet.css" rel="stylesheet" />

    </head>
    <body id="loginpg"> <!--full page background img -->
        <?php 
        include("header.php");
        include("process_poll.php");
        ?>

        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-lg-4 col-lg-offset-4">
                    <form id="postPoll" name="postPoll" method="post" action="" novalidate role="form">
                        <fieldset>
                            <legend>Poll</legend>
                            <?php if(!empty($error_message)) { ?>	
                            <div>
                                <p class="h5">
                                    <em>
                                        <?php echo $error_message; ?>  
                                    </em>                                    
                                </p>
                                
                            </div>
                            <?php ; } ?>
                            <p>Create new Poll: <a href="createPoll.php" class="btn btn-default">Create Poll</a></p>
                            <hr>
                            <div class="form-group">
                                <p class="h4">
                                    <?php 
                                    if ($error_message == ""){
                                        echo $pollQuestion; 
                                    }                             
                                    ?>
                                </p>        
                            </div>

                            <?php
                            //Display poll option if poll is available
                            if ($error_message == ""){
                                if (count($optionResults) > 0) {
                                    foreach ($optionResults as $option) {
                                        if ($questionType == "radio") {
                                            echo 
                                                '<div class="radio">
                                                    <label><input type="radio" name="pollRadio" value="'. $option["optionID"] .'"/>' . $option["optionTitle"] . '</label>
                                                </div>';
                                        } else {
                                            echo
                                                '<div class="form-check">
                                                    <input class="form-check-input" type="checkbox" value="" id="' . $option["optionID"] . '">
                                                    <label class="form-check-label" for="' . $option["optionID"] . '">
                                                    '. $option["optionTitle"] .'
                                                    </label>
                                                </div>';
                                        }
                                        
                                    }
                                    
                                    if ($isOtherAllowed == 1){
                                        if ($questionType == "radio") {
                                            echo 
                                            '<div class="form-inline">
                                                <input type="radio" name="pollRadio" value="other"/>
                                                <input type="text" class="form-control" name="other" placeholder="Other..." required/>
                                            </div>';
                                        } else {
                                            echo 
                                            '<div class="form-inline">
                                                <input class="form-check-input" type="checkbox" value=""/>
                                                <input type="text" class="form-control" name="other" placeholder="Other..." required/>
                                            </div>';
                                        }
                                        
                                    }
                                }
                            }
                            ?>
                            
                            <br/>
                            
                        </fieldset>
                    </form>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6 col-xs-offset-1 col-lg-3 col-lg-offset-4">
                    <?php
                    //Display EDIT and DELETE button if admin is logged in and poll is present
                        if (isset($_SESSION["user_login"]) && $error_message == "") {
                            // Edit button for logged in user
                            echo 
                            '<div class="text-right"><form action="editPoll.php" method="get">
                                <input type="hidden" id ="edit_pollID" name="edit_pollID" value="' . $pollID . '"/>
                                <input type="submit" id="editButton" class="btn btn-success" value="Edit"/>
                            </form></div>';
                        }
                    ?>
                </div>
                
                <div class="col-xs-4 col-lg-1">
                <?php 
                    if (isset($_SESSION["user_login"]) && $error_message == "") {
                        // Delete button for logged in user
                        echo 
                        '<div class="text-right"><form action="process_closePoll.php" method="post">
                            <input type="hidden" id="close_pollID" name="close_pollID" value="' . $pollID . '"/>
                            <input type="hidden" name="login_user" value="' . $login_user . '"/>
                            <input type="submit" id="closeButton" class="btn btn-danger" value="Close Poll" onclick="return confirm(\'Are you sure you want to close this poll?\');"/>           
                        </form></div>';
                    }
                ?>
                    
                </div>
                
            </div>
            
            <div class="row">
                <div class="col-xs-10 col-xs-offset-1 col-lg-4 col-lg-offset-4">
                    <hr/>
                    <?php 
                        if ($error_message == ""){
                            echo '<p><em>*The poll is <strong>only available for view</strong> here. To vote, please login to Sarawak Energy e-Tender mobile application.</em></p>';
                        }
                    ?>
                    
                    <hr/>
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