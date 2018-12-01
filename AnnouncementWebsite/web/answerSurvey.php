<?php 
include("header.php");
include("process_answerSurvey.php");

?>
<!DOCTYPE html>
<html data-ng-app="">
<head>
    <title>Pocket Tender | Answering Survey</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initialscale=1.0"/>
    <!-- Bootstrap -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/stylesheet.css" rel="stylesheet" />
    
</head>
<body id="pocketTenderBody"> <!--Photo by rawpixel.com from Pexels -->
    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-12" style="background-color:rgba(255, 255, 255, 0.7)">
                <div class="page-header">
                    <!-- POST from viewSurveyList.php form -->
                    <h3><?php echo $_POST["surveytitleinput"];?></h3>
                </div>
                <p id="isSuccess" class="text-success"><?php echo $resultMsg; ?></p>
                <p class="text-danger"><?php echo $errorMessage; ?></p>
                <form id="regForm" action="" method="post">
                <?php
                $currentquestionnumber = 0;
                
                if(count($result) > 0){
                    foreach($result as $key => $question){
                        $surveyJson = json_encode($question);
                        //if in array form, means answers from checkboxes, else is single answer
                        $currentquestionname = "question" . $currentquestionnumber . "answer";
                        $currentquestionID = $question["questionID"];
                        
                        if($question["questionType"] == "longsentence"){
                            echo 
                            '<div class="tab surveytab">
                                <div class="row contentRow">
                                    <div class="col-xs-12">
                                        <div class="row">
                                            <p><strong>' . $question["questionTitle"] . '</strong></p>
                                        </div>
                                        <div class="row">
                                            <input type="text" class="form-control" id="'.$currentquestionname.'" name="'.$currentquestionname.'" oninput="this.className = \'\'"/>
                                        </div>
                                    </div>
                                </div>
                            </div>';
                        }else if($question["questionType"] == "shortsentence"){
                            echo 
                            '<div class="tab surveytab">
                                <div class="row contentRow">
                                    <div class="col-xs-12">
                                        <div class="row">
                                            <p><strong>' . $question["questionTitle"] . '</strong></p>
                                        </div>
                                        <div class="row">
                                            <input type="text" class="form-control" id="'.$currentquestionname.'" name="'.$currentquestionname.'" oninput="this.className = \'\'"/>
                                        </div>
                                    </div>
                                </div>
                            </div>';
                        }else if($question["questionType"] == "checkboxes"){
                            $answerresult = getAnswersList($currentquestionID);
                            $answercount = 0;
                            echo 
                            '<div class="tab surveytab">
                                <div class="row contentRow">
                                    <div class="col-xs-12">
                                        <div class="row">
                                            <p><strong>' . $question["questionTitle"] . '</strong></p>
                                        </div>
                                        <div class="row">';
                                            foreach($answerresult as $key => $answers){
                                                //id for use in javascript control
                                                echo '
                                                <input type="checkbox" id= "'.$answercount.'" class="checkboxes" name="'.$currentquestionname.'[]" value="'. $answers["answerID"].'" onclick="toggleCheckboxNextPage()"/><label for="'.$answercount.'">'.$answers["answerTitle"].'</label>';
                                                $answercount++;
                                            }
                                    echo'
                                        <input type="hidden" name="checkboxinteract" id="checkboxinteract" oninput="this.className = \'\'">
                                        </div>
                                    </div>
                                </div>
                            </div>';
                        }else if($question["questionType"] == "dropdown"){
                            $answerresult = getAnswersList($currentquestionID);
                            echo 
                            '<div class="tab surveytab">
                                <div class="row contentRow">
                                    <div class="col-xs-12">
                                        <div class="row">
                                            <p><strong>' . $question["questionTitle"] . '</strong></p>
                                        </div>
                                        <div class="row">
                                            <select name="'.$currentquestionname.'" id="'.$currentquestionname.'" onchange="toggleDropdownNextPage(\'' . $currentquestionname . '\', \''. $currentquestionnumber .'\');">';
                                            echo '<option disabled selected value> -- select an option -- </option>';
                                            foreach($answerresult as $key => $answers){
                                                echo '
                                                <option value="'.$answers["answerID"].'">'.$answers["answerTitle"].'</option>
                                                ';
                                            }
                                    echo'   </select>
                                        <input type="hidden" name="dropdowninteract'. $currentquestionnumber .'" id="dropdowninteract'. $currentquestionnumber .'" oninput="this.className = \'\'">
                                        </div>
                                    </div>
                                </div>
                            </div>';
                        }else if($question["questionType"] == "radiobuttons"){
                            $answerresult = getAnswersList($currentquestionID);
                            echo 
                            '<div class="tab surveytab">
                                <div class="row contentRow">
                                    <div class="col-xs-12">
                                        <div class="row">
                                            <p><strong>' . $question["questionTitle"] . '</strong></p>
                                        </div>
                                        <div class="row">
                                            <select name="'.$currentquestionname.'" id="'.$currentquestionname.'" onchange="toggleRadioNextPage(\'' . $currentquestionname . '\', \''. $currentquestionnumber .'\');">';
                                            echo '<option disabled selected value> -- select an option -- </option>';   
                                            foreach($answerresult as $key => $answers){
                                                echo '
                                                <option value="'.$answers["answerID"].'">'.$answers["answerTitle"].'</option>
                                                ';
                                            }
                                    echo'   </select>
                                        <input type="hidden" name="radiobuttoninteract'. $currentquestionnumber .'" id="radiobuttoninteract'. $currentquestionnumber .'" oninput="this.className = \'\'">
                                        </div>
                                    </div>
                                </div>
                            </div>';
                        }
                        $currentquestionnumber++;
                    }
                }else{
                    echo '<div class="col-xs-12">
                            <div class="row">
                                <p>No Surveys Available</p>
                            </div>
                          </div>';
                }
                    
                
                
                
                ?>
                <!-- Re-post survey id so that even if this "post" overrides the $_POST[surveyidinput] from the earlier page, i can still access it -->
                <input type="hidden" name="surveyidinput" value="<?php echo $_POST["surveyidinput"];?>"/>
                <!-- Re-post survey title so that even if this "post" override the earlier $_POST[surveytitleinput] from the earlier page, it can still be accessed -->
                <input type="hidden" name="surveytitleinput" value="<?php echo $_POST["surveytitleinput"];?>"/>   
                <!-- $_POST[] to determine whether the form is submitted -->
                <input type="hidden" name="formsubmitted" value=""/>
                    
                    
                <div style="overflow:auto;">
                  <div style="float:right;">
                    <button type="button" id="prevBtn" onclick="nextPrev(-1)">Previous</button>
                    <button type="button" id="nextBtn" onclick="nextPrev(1)">Next</button>
                  </div>
                </div>
                <!-- Circles which indicates the steps of the form: -->
                <div style="text-align:center;margin-top:40px;">
                    <?php 
                        //since currentquestionnumber puts question 1 at 0
                        $totalnumberofquestion = $currentquestionnumber + 1;
                        for($i=1; $i<$totalnumberofquestion; $i++){
                            echo '<span class="step"></span>';
                        }
                    ?>    
                </div>
                
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
    <script src="../js/process_bookmark.js"></script>
    
    <!-- Control next page previous page of form -->
    <script type="text/javascript">
        
        var currentTab = 0; // Current tab is set to be the first tab (0)
        showTab(currentTab); // Display the current tab

        function showTab(n) {
          // This function will display the specified tab of the form ...
          var x = document.getElementsByClassName("tab");
          x[n].style.display = "block";
          // ... and fix the Previous/Next buttons:
          if (n == 0) {
            document.getElementById("prevBtn").style.display = "none";
          } else {
            document.getElementById("prevBtn").style.display = "inline";
          }
          if (n == (x.length - 1)) {
            document.getElementById("nextBtn").innerHTML = "Submit";
            //document.getElementById("nextBtn").name = "submitButton";
          } else {
            document.getElementById("nextBtn").innerHTML = "Next";
            /*if(document.getElementById("nextBtn").name == "submitButton"){
                document.getElementById("nextBtn").removeAttribute("name");
            }*/
          }
          // ... and run a function that displays the correct step indicator:
          fixStepIndicator(n)
        }

        function nextPrev(n) {
          // This function will figure out which tab to display
          var x = document.getElementsByClassName("tab");
          // Exit the function if any field in the current tab is invalid:
          if (n == 1 && !validateForm()) return false;
          // Hide the current tab:
          x[currentTab].style.display = "none";
          // Increase or decrease the current tab by 1:
          currentTab = currentTab + n;
          // if you have reached the end of the form... :
          if (currentTab >= x.length) {
            //...the form gets submitted:
            document.getElementById("regForm").submit();
            return false;
          }
          // Otherwise, display the correct tab:
          showTab(currentTab);
        }

        function validateForm() {
          // This function deals with validation of the form fields
          var x, y, i, valid = true;
          x = document.getElementsByClassName("tab");
          y = x[currentTab].getElementsByTagName("input");
          // A loop that checks every input field in the current tab:
          for (i = 0; i < y.length; i++) {
            // If a field is empty...
            if (y[i].value == "") {
              // add an "invalid" class to the field:
              y[i].className += " invalid";
              // and set the current valid status to false:
              valid = false;
            }
          }
          // If the valid status is true, mark the step as finished and valid:
          if (valid) {
            document.getElementsByClassName("step")[currentTab].className += " finish";
          }
          return valid; // return the valid status
        }

        function fixStepIndicator(n) {
          // This function removes the "active" class of all steps...
          var i, x = document.getElementsByClassName("step");
          for (i = 0; i < x.length; i++) {
            x[i].className = x[i].className.replace(" active", "");
          }
          //... and adds the "active" class to the current step:
          x[n].className += " active";
        }
        
        //On select dropdown, fill hidden input so that next page would be available
        function toggleDropdownNextPage(dropdownid, questionnumber){
            var selectDropdown = document.getElementById(dropdownid);
            var selectDropdownValue = selectDropdown.options[selectDropdown.selectedIndex].value;
            var hiddeninputid = "dropdowninteract" + questionnumber;
            document.getElementById(hiddeninputid).value = selectDropdownValue;
            
        }
        
        //On checked checkbox, fill hidden input so that next page would be available
        function toggleCheckboxNextPage(){
            var atleastoneischecked = false;
            var inputElements = document.getElementsByClassName("checkboxes");
            for(i=0; inputElements[i]; i++){
                if(inputElements[i].checked){
                    atleastoneischecked = true;
                }
            }
                
            if(atleastoneischecked == true){
                document.getElementById("checkboxinteract").value = "can proceed to next page";
            }else{
                document.getElementById("checkboxinteract").value = "";
            }
        
        }
        
        //On select radiobutton, fill hidden input so that next page would be available
        function toggleRadioNextPage(radiobuttonid, questionnumber){
            var selectRadio = document.getElementById(radiobuttonid);
            var selectRadioValue = selectRadio.options[selectRadio.selectedIndex].value;
            var hiddeninputid = "radiobuttoninteract" + questionnumber;
            document.getElementById(hiddeninputid).value = selectRadioValue;
            
        }
    </script>
</body>
</html>