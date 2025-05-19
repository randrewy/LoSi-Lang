# LoSi Lang

A demonstration project showcasing ANTLR4 parser code generation.

## Overview

LoSi (Logic Simple) is a minimalist language designed to support:

* Basic arithmetic operations `(+, -, *, /)`
* Boolean logic operations `(AND, OR, NOT)`
* Comparison operations `(>, >=, <, <=, =, !=)`
* Double literals and variables

## Features

* Type conversion: Double values are automatically cast to boolean when required. Positive values evaluate to true.
* Simplified variable handling: Variables are supported without explicit declarations, assuming definition elsewhere in the system.
* execution result is always a boolean value
