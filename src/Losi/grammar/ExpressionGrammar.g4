grammar ExpressionGrammar;

program
    : logicalExpression EOF
    ;

logicalExpression
    : mathToLogicalExpression                             # mathToLogicalExpr
    | logicalExpression AND logicalExpression             # andLogicalExpr
    | logicalExpression OR logicalExpression              # orLogicalExpr
    | NOT logicalExpression                               # notLogicalExpr
    | LPAREN logicalExpression RPAREN                     # parenLogicalExpr
    ;

mathToLogicalExpression
    : mathExpression                                                     # valueCastExpr
    | mathExpression op=(GT | LT | EQ | GTE | LTE | NEQ) mathExpression  # binaryComparisonExpr
    ;

mathExpression
    : mathAtom                                            # atomExpr
    | mathExpression op=(MUL | DIV) mathExpression        # mulDivExrp
    | MINUS mathAtom                                      # unaryMinusExpr
    | mathExpression op=(PLUS | MINUS) mathExpression     # plusMinusExrp
    ;
    
mathAtom
    : IDENTIFIER
    | NUMBER
    | LPAREN mathExpression RPAREN
    ;


// Operators
PLUS  : '+' ;
MINUS : '-' ;
MUL   : '*' ;
DIV   : '/' ;
GT    : '>' ;
LT    : '<' ;
EQ    : '=' ;
GTE   : '>=' ;
LTE   : '<=' ;
NEQ   : '!=' ;

// Logical operators
AND   : 'AND' ;
OR    : 'OR' ;
NOT   : 'NOT' ;

// Parentheses
LPAREN : '(' ;
RPAREN : ')' ;

// Literals and identifiers
IDENTIFIER : [a-zA-Z_][a-zA-Z0-9_]* ;
NUMBER     : [0-9]+ ('.' [0-9]+)? ;

// Whitespace
WS : [ \t\r\n]+ -> skip ;
