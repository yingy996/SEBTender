<?php

function sendNotification($message) {
    $url = "https://fcm.googleapis.com/fcm/send";

    $data = array(
        "to" => "/topics/news",
        "notification" => array(
            "title" => "FCM Message",
            "body" => $message
        )
    );

    $body = json_encode($data);
    $httpHeaders = array(
        "http" => array(
            "method" => "POST",
            "header" => array(
                "Content-Type: application/json", 
                "Authorization: key=AAAAFAZXRTg:APA91bEsWFrvWDYl0HkJJW6vNTs4o_z1WLvMCjNmPM0VuCgaNoTS3cSwVpgFNrLjd7wG4wMOvJTrnZ97jUxuemrj3uNOnpam4B_2b51oZ3hpmkRCHJ-6zwCA9FsK_7cjoxDJKBgDGCLi",
            ),
            #"header" => "Authorization: key=AAAAFAZXRTg:APA91bEsWFrvWDYl0HkJJW6vNTs4o_z1WLvMCjNmPM0VuCgaNoTS3cSwVpgFNrLjd7wG4wMOvJTrnZ97jUxuemrj3uNOnpam4B_2b51oZ3hpmkRCHJ-6zwCA9FsK_7cjoxDJKBgDGCLi",
            "content" => $body,
        )   
    );

    /*$httpHeaders["http"]["method"] = "POST";
    $httpHeaders["http"]["header"] = "Content-Type: application/json\r\n";
    $httpHeaders["http"]["header"] .= "Authorization: key=AAAAFAZXRTg:APA91bEsWFrvWDYl0HkJJW6vNTs4o_z1WLvMCjNmPM0VuCgaNoTS3cSwVpgFNrLjd7wG4wMOvJTrnZ97jUxuemrj3uNOnpam4B_2b51oZ3hpmkRCHJ-6zwCA9FsK_7cjoxDJKBgDGCLi\r\n";
    $httpHeaders["http"]["content"] = $contentBody;*/

    $context = stream_context_create($httpHeaders);

    $response = file_get_contents($url, false, $context);

    //echo "Response: " . $response;
}
    
?>