grammar Enum;

prog: usingOrNamespaceDirective;

usingOrNamespaceDirective: (keywords (namespaceName '.'?)* ';'? )*? enumDeclaration+;

enumDeclaration: keywords* ID  statementOrAnnotation* ;

statementOrAnnotation: stat | dataAnnotation;

stat:  assign ','? ;

assign: ID ('=' expr)?;

dataAnnotation: '[' ((ID | '(' | ')' | ';' | '+' | '=' | TEXT) '.'?)* ']' ;

expr: 
	  NUMBER
    | ID
    | TEXT
    ;

keywords: 
	  PUBLIC
	| STATIC
	| STRING
	| USING
	| NAMESPACE
	| ENUM
	;

namespaceName : ID ;
region : '#' ID* ;

SINGLE_LINE_DOC_COMMENT: '///' InputCharacter*    -> skip;
DELIMITED_DOC_COMMENT:   '/**' .*? '*/'           -> skip;
SINGLE_LINE_COMMENT:     '//'  InputCharacter*    -> skip;
DELIMITED_COMMENT:       '/*'  .*? '*/'           -> skip;

fragment InputCharacter:       ~[\r\n\u0085\u2028\u2029];

// keywords
PUBLIC:		'public';
USING:		'using';
NAMESPACE:	'namespace';
ENUM:		'enum';
STATIC:		'static';
STRING:		'string';
CONST:		'const';



ID  :   [a-zA-Z0-9_]+ ;
TEXT:	'"' .*? '"' ;
COMMA_SEPARATOR :	','						-> skip ;
NEWLINE :			('\n' | '\r' | '\r\n')	-> skip ;
WS :				[ \t]+					-> skip ;
CURLY_BRACKETS :	('{' | '}')				-> skip ;
