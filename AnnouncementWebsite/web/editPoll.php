<?php 
include("header.php");
include("process_editPoll.php");
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
                    <form id="publishPoll" name="publishPoll" method="post" action="" novalidate role="form">
                        <fieldset>
                            <legend>Edit Poll</legend>
                            <?php if(!empty($success_message)) { ?>	
                            <div class="alert alert-success">
                                <?php if(isset($success_message)) echo $success_message; ?></div>
                            <?php } ?>
                            <?php if(!empty($error_message)) { ?>	
                            <div class="alert alert-danger"><?php if(isset($error_message)) echo $error_message; ?></div>
                            <?php } ?>
                            <div class="form-group">
                                <label for="question">Poll Question:</label>
                                <input type="text" class="form-control" id="question" name="question" placeholder="Enter Poll Question" <?php if(isset($pollQuestion)){ echo 'value="'.$pollQuestion.'"';} ?> required/><span class="error"><?php if($pollQuestion_Error != "") echo "<p class='alert alert-danger'>" . $pollQuestion_Error . "</p>";?></span>
                            </div>

                            <div class="form-group">
                                <label for="option_number">Number of option:</label>
                                <input type="number" min="2" max="20" class="form-control" id="option_number" name="option_number" <?php if(isset($pollOptionNumber)){ echo 'value="'.$pollOptionNumber.'"';} ?> placeholder="Enter Number of Options"/>
                                <button name="addOptionButton" id="addOptionButton" value="addOptionButton" class="btn btn-default">Add Option</button><span class="error"><?php if($pollOptionNumber_Error != "") echo "<p class='alert alert-danger'>" . $pollOptionNumber_Error . "</p>";?></span>
                            </div>
                            
                            <div id="optionsDiv">
                            <?php 
                                if(!empty($_POST["option_number"])) {
                                    //add option fields according to user's input
                                    $optionNumber = (int) $_POST["option_number"];

                                    if ($optionNumber < 2) {
                                        echo '<div class="alert alert-danger">"The number of option must be at least 2."</div>';
                                    } else {
                                        for($i = 1; $i <= $optionNumber; $i++) {
                                            if ($i == $optionNumber){
                                                //Display the "Other" option is the option is allowed
                                                if ($isOtherAllowed == 1 || !empty($_POST["other"])){
                                                    echo 
                                                    '<div class="form-group" id="divOption' . $i . '">
                                                        <label for="option'. $i .'">Option '. $i . ':</label>
                                                        <input type="hidden" name="other" value="other"/>';

                                                        echo '<input type="text" disabled class="form-control" id="option'. $i .'" name="other" placeholder="Other..."/>';

                                                        echo '<button name="deleteOptionButton' . $i . '" value="other" onclick="return confirm(\'Are you sure you want to delete this option?\');" class="btn btn-danger">Delete</button>';

                                                    echo '</div>';
                                                } else {
                                                    //Display checkbox to allow user to set the option as "Other" option
                                                    echo 
                                                    '<div class="form-group" id="divOption' . $i . '">
                                                        <label for="option'. $i .'">Option '. $i . ':</label>
                                                        <input type="hidden" name="optionID' . $i . '"';
                                                        if (isset($pollOptions["optionID" . $i])) {
                                                            echo 'value="' . $pollOptions["optionID" . $i] . '"'; 
                                                        }
                                                        echo '/>';
                                                        echo '<input type="text" class="form-control" id="option'. $i .'" name="option' . $i . '"';
                                                        if (isset($pollOptions["option" . $i])) {
                                                            echo 'value="' . $pollOptions["option" . $i] . '"';
                                                        }
                                                        echo 'required/>';
                                                    if (!empty($pollOptions["optionID" . $i])) {
                                                        echo '<button name="deleteOptionButton' . $i . '" value="' . $pollOptions["optionID" . $i] .'" onclick="return confirm(\'Are you sure you want to delete this option?\');" class="btn btn-danger">Delete</button>';
                                                    } else {
                                                        if ($i == $optionNumber) {
                                                            //Display checkbox when user 
                                                            echo 
                                                            '<div class="form-check">
                                                                  <input class="form-check-input" type="checkbox" id="isOther" name="isOther" value="isOther" onChange="document.getElementById(\'option'. $i .'\').disabled = this.checked;">
                                                                  <label class="form-check-label" for="isOther">
                                                                    Set as "Other" input
                                                                  </label>
                                                            </div>';
                                                        }                                                
                                                    }

                                                    echo '<span class="error">';

                                                    if(isset($optionErrors["option" . $i])) {
                                                        echo "<p class='alert alert-danger'>" . $optionErrors["option" . $i] . "</p>";
                                                    }
                                                    echo '</span></div>';
                                                }
                                            } else {
                                                echo 
                                                '<div class="form-group" id="divOption' . $i . '">
                                                    <label for="option'. $i .'">Option '. $i . ':</label>
                                                    <input type="hidden" name="optionID' . $i . '"';
                                                    if (isset($pollOptions["optionID" . $i])) {
                                                        echo 'value="' . $pollOptions["optionID" . $i] . '"'; 
                                                    }
                                                    echo '/>';
                                                    echo '<input type="text" class="form-control" id="option'. $i .'" name="option' . $i . '"';
                                                    if (isset($pollOptions["option" . $i])) {
                                                        echo 'value="' . $pollOptions["option" . $i] . '"';
                                                    }
                                                    echo 'required/>';
                                                if (!empty($pollOptions["optionID" . $i])) {
                                                    echo '<button name="deleteOptionButton' . $i . '" value="' . $pollOptions["optionID" . $i] .'" onclick="return confirm(\'Are you sure you want to delete this option?\');" class="btn btn-danger">Delete</button>';
                                                } else {
                                                    if ($i == $optionNumber) {
                                                        //Display checkbox when user 
                                                        echo 
                                                        '<div class="form-check">
                                                              <input class="form-check-input" type="checkbox" id="isOther" name="isOther" value="isOther" onChange="document.getElementById(\'option'. $i .'\').disabled = this.checked;">
                                                              <label class="form-check-label" for="isOther">
                                                                Set as "Other" input
                                                              </label>
                                                        </div>';
                                                    }                                                
                                                }

                                                echo '<span class="error">';

                                                if(isset($optionErrors["option" . $i])) {
                                                    echo "<p class='alert alert-danger'>" . $optionErrors["option" . $i] . "</p>";
                                                }
                                                echo '</span></div>';
                                            }
                                        }
                                        
                                        if ($isOtherAllowed == 1){
                                            
                                        }
                                    }
                                    
                                }
                            ?>
                            </div>
                            <p><input type="submit" name="updatePollButton" class="btn btn-default" id="updatePollButton" value="Update"/></p>

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