Hello {{ model.FirstName | string.append " (G.)" }} {{ model.LastName }} 
{{ NotExistingInModel }}
{{ MySimpleFunction }}
Result of custom function: {{ MyCustomFunction 123 456 789 }}
