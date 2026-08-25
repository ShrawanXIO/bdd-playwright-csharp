Feature:Login
    As a Saucedemo user
    I want to log in with valid credentials. 
    So that I can access the store 
    
    @smoke
    Scenario: successful login with a valid user
        Given I am on a Saucedemo login page
        When I log in with username "standard_user" And password "secret_sauce"  
        Then I should see the inventory page
    

    @regression
    Scenario Outline: Login attempts with invalid credentials
        Given I am on a Saucedemo login page
        When I log in with username "<username>" And password "<password>"
        Then I should see an error message "<errorMessage>"

    Examples:
        | username         | password        | errorMessage                                                               |
        | astandard_user    | wrong_password   | Epic sadface: Username and password do not match any user in this service |
        | locked_out_user    | secret_sauce     | Epic sadface: Sorry, this user has been locked out.                        |
        | standard_user       | wrong_password   | Epic sadface: Username and password do not match any user in this service |

   

