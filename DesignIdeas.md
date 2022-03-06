# Main aspects + reason behind them
- specifying options
    - options are added one by one
        - in comparison with passing collection of pre-defined options
            - allows greater flexibility for user
- defining synonyms
    - when adding option we add all its synonyms
        - adding synonyms later would be more error-prone and it wouldn't simplify usage
    - differentiating between short/long option
        - we assume that short option consists of 1 char only and long option consist of 2+ chars
            - user doesn't have to specify `-` or `--` in option name
- option parameter
    - we use enum to differentiate between `may/may not/must` accept param
        - is extensible for multiple params (not required)
- option parameter type
    - we use inheritance
        - allows only certain types
        - is extensible
        - allows associating restrictions with specific type
        - simple type check: using inherited validation method
    - other ideas
        - use generic type: allows all types (such as complex objects) which we couldn't parse
- parsed result
    - result.Get(OptName)
        - string search means lookup errors will be detected at runtime and not compilation
        1. return Dictionary with result
            - can't associate exact type with option name
                1. user would have to cast value by himself => bad
                2. use inheritance with sth like .GetValue() => user has to get value dictionary result
                    - problem
                        - inheritance => must be generic => can't be saved in collection
                        - without inheritance can't return exact type
        2. Get method implemented in library
            - too many responsibilities for 1 class
            - result has to carry whole configuration with it
        3. Create new class with Get method for result
            - prob. best solution (avoids drawbacks from previous)
    - callback
        - option ordering might change behavior
        - usually it will only set result to some variable
            - lot of code for such a simple action
    - named var ref: `AddOption()` would return reference to parsed result
        - leads to too many variables, unstructured
        - however, more type-safe
    - user specifies options through class
        - must use reflection
- access values of plain arguments
    - we require name for each plain argument
        - for description in HELP
        - for value access
    - `The delimiter may be omitted unless a plain argument starts with -.`: probably implementation detail
- help
    - generated from option name, description, ...

# todo
- je k necemu `ArgumentOptionBase` potomek obou ??? ------------
    - dava cela hierarchie argumentu/optionu smysl?-------------

- vyjimky?
    - typy
        - kolize jmen optionu
        - neposkytnuti povinneho optionu
            - jak budeme kontrolovat - souvisi s *vnitrni reprezentaci* (impl. detail, ale bylo by dobre rozmyslet)
        - spatny typ argumentu nebo nevalidni argument
    - vyhazovat hned (napr. 1. spatny typ) NEBO ukladat chyby a informovat o vsech najednou?
