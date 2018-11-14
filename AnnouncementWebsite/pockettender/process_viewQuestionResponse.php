<?php 
require_once("../dbcontroller.php");
$db_handle = new DBController();

// Get question title based on questionID
$query = $db_handle->getConn()->prepare("SELECT questionTitle FROM survey_question WHERE questionID = :questionID");
$query->bindParam(":questionID", $_GET['questionID']);
$query->execute();
$question = $query->fetchAll(); 

// Get answer choices based on questionID (DROPDOWN)
if($_GET['questionType'] == 'dropdown')
{
    $query = $db_handle->getConn()->prepare("SELECT survey_questionanswer.answerTitle as scale FROM survey_response_answer JOIN survey_questionanswer ON survey_response_answer.answerID = survey_questionanswer.answerID WHERE survey_response_answer.questionID = :questionID");
    $query->bindParam(":questionID", $_GET['questionID']);
    $query->execute();
    $response_answers = $query->fetchAll(); 
    
    $scale1 = 0.0; 
    $scale2 = 0.0; 
    $scale3 = 0.0; 
    $scale4 = 0.0; 
    $scale5 = 0.0;
    $total_response = count($response_answers);

    if(count($response_answers) > 0)
    {
        $x = 0;

        while($x < $total_response) {

            if ($response_answers[$x]["scale"] == "1")
            {
                $scale1 += 1.0;
            } elseif ($response_answers[$x]["scale"] == "2")
            {
                $scale2 += 1.0;
            } elseif ($response_answers[$x]["scale"] == "3")
            {
                $scale3 += 1.0;
            } elseif ($response_answers[$x]["scale"] == "4")
            {
                $scale4 += 1.0;
            } elseif ($response_answers[$x]["scale"] == "5")
            {
                $scale5 += 1.0;
            }

            $x++;
        } 
    }


}

// Get question response answer based on questionID (LONGSENTENCE)
if($_GET['questionType'] == 'longsentence')
{
    $query = $db_handle->getConn()->prepare("SELECT survey_response_answer.text_answer as text_answer,survey_response.userID as userID ,survey_response.dateSubmitted as dateSubmitted FROM survey_response_answer JOIN survey_response ON survey_response_answer.responseID = survey_response.responseID WHERE survey_response_answer.questionID = :questionID ORDER BY dateSubmitted DESC");
    $query->bindParam(":questionID", $_GET['questionID']);
    $query->execute();
    $response_answers = $query->fetchAll(); 
}

?>
