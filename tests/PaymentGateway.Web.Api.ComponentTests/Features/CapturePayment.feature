Feature: CapturePayment
Capturing a card payment

    Scenario: Invalid request authorising a card payment
        Given an invalid payment request
        When a payment request is sent
        Then a BadRequest response is returned
        And the error code is InvalidAmount and message is Amount is Invalid
        And the error code is InvalidCardNumber and message is Invalid Card Number