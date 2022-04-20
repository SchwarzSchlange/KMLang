# KMLang
### Much improved version of the Vann language.

Documentation : 

```ruby
-----------------
Print :
out(STRING_VALUE)
out($variable)
-----------------
Input :
in VARIABLE_NAME
-----------------
Delay :
wait{MS_TIME}
-----------------
Function Create:
block NAME(TO_DO1 | TO_DO2 | TO_DO3)[PARAMETERS]
-----------------
Function Call:
call NAME[PARAMETERS] (Even if there is no parameter, something must be written in this field (Like : NULL)) 
-----------------
Print with Math Expression :
out({MATH_EXPRESSION})
Exp: out(30+10 equals to : {30+10})
-----------------
Creating a Variable :
push NAME(VALUE) (Math expression hasn't been yet added)
Exp : push name(Hallo I'm Kaan)
Exp : push x(10)
-----------------
If Expression :
if (STRING_VALUE OR ANY) = (STRING_VALUE OR ANY) ? COMMAND (Use functions to do many commands)
Exp with string or any : if (30) = (30) ? out(True!)

Exp with variable:
push x(30)
push y(30)
if $x = (30) ? out(True!)
if $x = $y ? out(True!)

Exp with Function:

block write(out(first text)|out(second text))[]
if $x = $y ? call sum[null]

-----------------
Clear All :
clear
-----------------
Exit Program :
exit
-----------------

```
