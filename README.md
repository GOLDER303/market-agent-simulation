## Milestone 2
### Class diagrams
```mermaid
classDiagram
    direction LR

    %% Abstract base class
    class Entity {
        <<abstract>>
        - position: Point2D
        + getPosition(): Point2D
    }

    class Agent {
        - speed: double
        + move(timeStep: double)
    }

    class Customer {
        - shoppingList: List~String~
        - perception: double
        - patience: double
        - patienceDecayRate: double
        - persuasionSusceptibility: double
        - goal: Point2D | Null
        + setGoal(Entity)
        + clearGoal()
        + perceive(): List~Entity~
        + requestHelp()
        + updatePatience(timeStep: double)
        + pickUpProduct(product: Product)
        + addProductToShoppingList(product: String)
    }

    class Staff {
        - productKnowledge: double
        - persuasionFactor: double
        + assistCustomer(customer: Customer)
        + persuadeCustomer(customer: Customer)
    }

    class Product {
        - name: String
    }

    class Market {
        - customers: List~Customer~
        - staff: List~Staff~
        - products: List~Product~
        - entryPoint: Point2D
        - exitPoint: Point2D
        - bounds: BoundingBox
        + getProducts()
        + getStaff()
        + spawnStaff()
        + spawnProduct()
        + spawnCustomer()
        + simulationStep(timeStep: double)
        + removeCustomer(Customer: Customer)
    }

    Entity <|-- Agent
    Entity <|-- Product
    Agent <|-- Customer
    Agent <|-- Staff
    Market .. Customer
    Market .. Staff
    Market .. Product
```

### Sequence diagrams
#### Customer finds product by itself
```mermaid
sequenceDiagram
    participant Market
    actor Customer

    Market->>Customer: simulationStep(timeStep)
    Customer->>Customer: perceive()
    Customer->>Market: getProducts()
    Market-->>Customer: list of nearby Products
    Customer->>Customer: decide to go toward Product
    Customer->>Customer: setGoal(Product)
    loop while not at product
        Customer->>Customer: move(timeStep)
    end
    create participant Product
    Customer->>Product: pickUpProduct()
    Customer->>Customer: remove from shoppingList

```

#### Customer requests help from staff
```mermaid
sequenceDiagram
    participant Market
    actor Customer

    Market->>Customer: simulationStep(timeStep)
    Customer->>Customer: perceive()
    Customer->>Market: getStaff()
    Market-->>Customer: list of nearby Staff
    Customer->>Customer: decide to requestHelp()
    create actor Staff
    Customer->>Staff: requestHelp()
    Staff->>Staff: check product knowledge
    alt Staff knows where product is
        Staff->>Customer: persuade to add another product
        Customer->>Customer: maybe add extra product
        Customer->>Customer: addProductToShoppingList(Product)
        destroy Staff
        Staff->>Customer: setGoal(Product)
        loop move toward product
            Customer->>Customer: move(timeStep)
        end
        create participant Product
        Customer->>Product: pickUpProduct()
    end
```

### Object diagram
```mermaid
classDiagram
    direction LR

    %% Instances (simulated using class names)

    class market1 {
        entryPoint = (0,0)
        exitPoint = (100,100)
    }

    class product1 {
        name = "Apples"
    }

    class product2 {
        name = "Bread"
    }

    class staff1 {
        productKnowledge = 0.9
        persuasionFactor = 0.75
    }

    class customer1 {
        shoppingList = ["Apples", "Bread"]
        perception = 0.8
        patience = 1.0
        patienceDecayRate = 0.05
        persuasionSusceptibility = 0.6
        goal = (20, 30)
    }

    class customer2 {
        shoppingList = ["Bread"]
        perception = 0.7
        patience = 0.9
        patienceDecayRate = 0.04
        persuasionSusceptibility = 0.4
        goal = null
    }

    %% Associations (simulated)
    market1 --> customer1
    market1 --> customer2
    market1 --> staff1
    market1 --> product1
    market1 --> product2
    customer1 --> product1
    customer1 --> product2
    staff1 --> customer1
```

### State Machine Diagrams
#### Customer State Machine Diagram
```mermaid
stateDiagram-v2
    direction LR
    [*] --> Idle
    Idle --> Searching : hasShoppingList
    Searching --> HeadingToProduct : seesProduct
    HeadingToProduct --> PickingProduct : reachedProduct
    PickingProduct --> Idle : productPicked
    Searching --> RequestingHelp : needsAssistance
    RequestingHelp --> Persuaded : persuaded
    RequestingHelp --> Idle : notPersuaded
    Idle --> Exiting : shoppingListEmpty or patienceDepleted
    Exiting --> [*]
```
