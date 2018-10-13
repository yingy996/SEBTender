<?php

class scrapped_tender
{
    public $reference;
    public $title;
    public $category;
    public $originatingSource;
    public $tenderSource;
    public $agency;
    public $tendererClass;
    public $closingDate;
    public $startDate;
    public $docInfoJson;
    public $originatorJson;
    public $fileLink;
}

class tenderDocInfo {
    public $bidCloseDate;
    public $feeBeforeGST;
    public $feeGST;
    public $feeAfterGST;
}

class originatorInfo {
    public $name;
    public $officePhone;
    public $extension;
    public $mobilePhone;
    public $email;
    public $fax;
}
?>