<!DOCTYPE html>
<html data-ng-app="">
    <head>
        <title>Pocket Tender</title>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initialscale=1.0"/>
        <!-- JQuery -->
         <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <!-- Bootstrap -->
        <link href="../css/bootstrap.min.css" rel="stylesheet" />
        <!--<link href="../css/stylesheet.css" rel="stylesheet" />-->
        <!-- Javascript-->
        <script type="text/javascript" src="../js/surveyscript.js"></script> 


    </head>
    <body id="loginpg"> <!--full page background img -->
        <?php 
        @ob_start();
        include("header.php");
        include("process_postSurvey.php");
        ?>

        <div class="container-fluid">
            <form id="postSurvey" name="postSurvey" method="post" action="" novalidate role="form">
            <fieldset>
            <div class="form-group">
                <div class="col-xs-4 col-xs-offset-4">
                        
                            <legend>Posts Survey</legend>
                            <?php if(!empty($success_message)) { ?>	
                            <div class="alert alert-success">
                                <?php if(isset($success_message)) echo $success_message; ?></div>
                            <?php } ?>
                            <?php if(!empty($error_message)) { ?>	
                            <div class="alert alert-danger"><?php if(isset($error_message)) echo $error_message; ?></div>
                            <?php } ?>
                            <div class="form-group">
                                <label for="title">Survey Title:</label>
                                
                                <input type="text" class="form-control" id="title" name="title" placeholder="Enter Survey Title" required/>
                                
                                <span class="error"><?php if($survey_titleerr != "") echo "<p class='alert alert-danger'>" . $survey_titleerr . "</p>";?></span>
                                <br/>
                                <label for="content">Survey Description:</label>
                                <input type="text" class="form-control" id="content" name="content" placeholder="Enter Survey Description" required/>
                                <span class="error"><?php if($survey_descriptionerr != "") echo "<p class='alert alert-danger'>" . $survey_descriptionerr . "</p>";?></span>
                            </div>
                            
                        
                </div>
                       
            </div>
                            
            <div class="form-group">
                            <div class="col-xs-12" id="surveyInput">
                                
                                <legend>Survey Questions</legend>
                                <span class="error"><?php if($question_titleerr != "") echo "<p class='alert alert-danger'>" . $question_titleerr . "</p>";?></span>
                                <span class="error"><?php if($answer_titleerr != "") echo "<p class='alert alert-danger'>" . $answer_titleerr . "</p>";?></span>
                                <span class="error"><?php if($end_date != "") echo "<p class='alert alert-danger'>" . $end_dateerr . "</p>";?></span>
                                <p>
                                    <input type="button" value="Add Question" OnClick="addRow('surveyInput','questionDIV','answerDIV')"/>
                                    <input type="button" value="Add Answer" OnClick="addAnswerRow()"/>
                                    <input type="button" value="Delete Question" OnClick="deletequestionRow('questionDIV')"/>
                                    <input type="button" value="Delete Answer" OnClick="deleteanswerRows()"/>
                                </p>
                                
                                <div id="questionDIV" style="display: none;">
                                    <input type="checkbox" required="required" name="quescheckbox" name="quescheckbox" id="quescheckbox"/>
                                    
                                    <label for="question_title">Question title:</label>
                                    
                                    <input type="text" id="question_title" name="question_title" required="required" placeholder="question title" size = "100"/>
                                    
                                    <div class="dropdown col-xs-offset-1">
                                    <select id="answertype" 
									name="answertype" onchange="answertypechange(this)">
                                      <option value="shortsentence">Short Sentence</option>
                                      <option value="longsentence">Long Paragraph</option>
                                      <option value="dropdown">Dropdown</option>
                                      <option value="checkboxes">Checkboxes</option>
                                      <option value="radiobuttons">Radio Button</option>
                                    </select>
                                    </div>
                                    
                                        <div id="answerDIV" class="col-xs-offset-1" >
                                            
                                            <input type="checkbox" required="required" name="anscheckbox" id="anscheckbox"/>
                                            <input type="text" id="answer_title" name="answer_title" required="required" placeholder="answer" size="100"/>
                                        </div>
                                    
                                
                                
                                </div>
                                
                                <div id="questionDIV00">
                                    <input type="checkbox" required="required" name="quescheckbox00" id="quescheckbox00"/>
                                    
                                    <label for="question_title">Question title:</label>
                                    
                                    <input type="text" name="question_title00" id="question_title00" required="required" placeholder="question title" size = "100"/>
                                    
                                    
                                    <div class="dropdown col-xs-offset-1">
                                    <select id="answertype00" name="answertype00" onchange="answertypechange(this)">
                                      <option value="shortsentence">Short Sentence</option>
                                      <option value="longsentence">Long Paragraph</option>
                                      <option value="dropdown">Dropdown</option>
                                      <option value="checkboxes">Checkboxes</option>
                                      <option value="radiobuttons">Radio Button</option>
                                    </select>
                                    </div>
                                        <div id="answerDIV0000" class="col-xs-offset-1">
                                            <input type="checkbox" required="required" name="chk2[]" id="anscheckbox0000"/>
                                            <input type="text" name="answer_title0000" id="answer_title0000" required="required" placeholder="answer" size="100"/>
                                        </div>
                                    
                                </div>      
                            </div>
                <div id="datefields" name="datefields" class="col-xs-12">

								
                    <label for="enddate">End Date:</label>
                    <input type="date" id="enddate" name="enddate"/>
                        <br/>
                </div>
                <div class="col-xs-12">
                    <p><input type="submit" name="submitsurvey" class="btn btn-default" id="submitsurvey" value="Posts Survey"/></p>
                </div>
                </div>
            </fieldset>
            </form>
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