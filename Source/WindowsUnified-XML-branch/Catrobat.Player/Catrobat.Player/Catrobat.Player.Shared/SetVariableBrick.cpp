#include "pch.h"
#include "SetVariableBrick.h"

SetVariableBrick::SetVariableBrick(FormulaTree *variableFormula,std::shared_ptr<Script> parent)
	: VariableManagementBrick(TypeOfBrick::SetVariableBrick, variableFormula, parent)
{
}

void SetVariableBrick::Execute()
{
    // TODO: typecheck and logic
	m_variable->SetValue(m_variableFormula->Value());
}
