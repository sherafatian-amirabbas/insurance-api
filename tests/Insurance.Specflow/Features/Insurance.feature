Feature: Insurance

Background: 
	Given the following Products were available
		| Id | Name                                 | SalesPrice | ProductTypeId |
		| 1  | Canon EOS 77D + 18-55mm IS STM       | 0          | 1             |
		| 2  | Lenovo Chromebook C330-11 81HY000MMH | 500        | 2             |
		| 3  | OnePlus 8 Pro 128GB Black 5G         | 2000       | 3             |
		| 4  | Sony CyberShot 1                     | 0          | 4             |
		| 5  | Sony CyberShot 2                     | 0          | 4             |
	
	And the following ProductTypes were available
		| Id | Name            | CanBeInsured |
		| 1  | SLR cameras     | true         |
		| 2  | Laptops         | true         |
		| 3  | Smartphones     | true         |
		| 4  | Digital cameras | true         |

Scenario: Order Insurance Calculation
	Given the following products were available in the order
		| Id |
		| 1  |
		| 2  |
		| 3  |
		| 4  |
		| 5  |
	When insurance is calculated for the order
	Then the insurance value is 4500
