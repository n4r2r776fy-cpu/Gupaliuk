classDiagram
    class ProductService {
        +AddProduct(name, price, stock)
    }

    class IProductValidator {
        <<interface>>
        +Validate(name, price)
    }

    class IProductRepository {
        <<interface>>
        +Save(name, price, stock)
    }

    class ILogger {
        <<interface>>
        +Log(message)
    }

    class IStockNotifier {
        <<interface>>
        +NotifyLowStock(name, stock)
    }

    ProductService --> IProductValidator
    ProductService --> IProductRepository
    ProductService --> ILogger
    ProductService --> IStockNotifier
