using System.Windows.Input;

namespace AutoTest.Core
{
    public static class UICommands
    {
        #region Main commands

        static RoutedCommand _newProjectCommand;
        public static RoutedCommand NewProject
        {
            get
            {
                if (_newProjectCommand == null)
                    _newProjectCommand = new RoutedCommand();
                return _newProjectCommand;
            }
        }

        static RoutedCommand _openProjectCommand;
        public static RoutedCommand OpenProject
        {
            get
            {
                if (_openProjectCommand == null)
                    _openProjectCommand = new RoutedCommand();
                return _openProjectCommand;
            }
        }

        static RoutedCommand _saveProjectCommand;
        public static RoutedCommand SaveProject
        {
            get
            {
                if (_saveProjectCommand == null)
                    _saveProjectCommand = new RoutedCommand();
                return _saveProjectCommand;
            }
        }

        static RoutedCommand _saveAsProjectCommand;
        public static RoutedCommand SaveAsProject
        {
            get
            {
                if (_saveAsProjectCommand == null)
                    _saveAsProjectCommand = new RoutedCommand();
                return _saveAsProjectCommand;
            }
        }

        static RoutedCommand _compileProjectCommand;
        public static RoutedCommand CompileProject
        {
            get
            {
                if (_compileProjectCommand == null)
                    _compileProjectCommand = new RoutedCommand();
                return _compileProjectCommand;
            }
        }

        static RoutedCommand _runAllCommand;
        public static RoutedCommand RunAll
        {
            get
            {
                if (_runAllCommand == null)
                    _runAllCommand = new RoutedCommand();
                return _runAllCommand;
            }
        }

        static RoutedCommand _runCurrentStepCommand;
        public static RoutedCommand RunCurrentStep
        {
            get
            {
                if (_runCurrentStepCommand == null)
                    _runCurrentStepCommand = new RoutedCommand();
                return _runCurrentStepCommand;
            }
        }

        static RoutedCommand _runCurrentTestCaseCommand;
        public static RoutedCommand RunCurrentTestCase
        {
            get
            {
                if (_runCurrentTestCaseCommand == null)
                    _runCurrentTestCaseCommand = new RoutedCommand();
                return _runCurrentTestCaseCommand;
            }
        }

        static RoutedCommand _stopCommand;
        public static RoutedCommand Stop
        {
            get
            {
                if (_stopCommand == null)
                    _stopCommand = new RoutedCommand();
                return _stopCommand;
            }
        }

        #endregion

        #region Test case commands

        static RoutedCommand _moveUpCommand;
        public static RoutedCommand MoveUp
        {
            get
            {
                if (_moveUpCommand == null)
                    _moveUpCommand = new RoutedCommand();
                return _moveUpCommand;
            }
        }

        static RoutedCommand _moveDownCommand;
        public static RoutedCommand MoveDown
        {
            get
            {
                if (_moveDownCommand == null)
                    _moveDownCommand = new RoutedCommand();
                return _moveDownCommand;
            }
        }

        static RoutedCommand _editTestCaseCommand;
        public static RoutedCommand EditTestCase
        {
            get
            {
                if (_editTestCaseCommand == null)
                    _editTestCaseCommand = new RoutedCommand();
                return _editTestCaseCommand;
            }
        }

        static RoutedCommand _lockCommand;
        public static RoutedCommand Lock
        {
            get
            {
                if (_lockCommand == null)
                    _lockCommand = new RoutedCommand();
                return _lockCommand;
            }
        }

        static RoutedCommand _unlockCommand;
        public static RoutedCommand Unlock
        {
            get
            {
                if (_unlockCommand == null)
                    _unlockCommand = new RoutedCommand();
                return _unlockCommand;
            }
        }

        static RoutedCommand _addTestCaseCommand;
        public static RoutedCommand AddTestCase
        {
            get
            {
                if (_addTestCaseCommand == null)
                    _addTestCaseCommand = new RoutedCommand();
                return _addTestCaseCommand;
            }
        }

        static RoutedCommand _addChildTestCaseCommand;
        public static RoutedCommand AddChildTestCase
        {
            get
            {
                if (_addChildTestCaseCommand == null)
                    _addChildTestCaseCommand = new RoutedCommand();
                return _addChildTestCaseCommand;
            }
        }

        static RoutedCommand _addActionCommand;
        public static RoutedCommand AddAction
        {
            get
            {
                if (_addActionCommand == null)
                    _addActionCommand = new RoutedCommand();
                return _addActionCommand;
            }
        }

        #endregion

        #region Global variable commands

        static RoutedCommand _editGlobalVariableGroupCommand;
        public static RoutedCommand EditGlobalVariableGroup
        {
            get
            {
                if (_editGlobalVariableGroupCommand == null)
                    _editGlobalVariableGroupCommand = new RoutedCommand();
                return _editGlobalVariableGroupCommand;
            }
        }

        static RoutedCommand _addGlobalVariableGroupCommand;
        public static RoutedCommand AddGlobalVariableGroup
        {
            get
            {
                if (_addGlobalVariableGroupCommand == null)
                    _addGlobalVariableGroupCommand = new RoutedCommand();
                return _addGlobalVariableGroupCommand;
            }
        }

        static RoutedCommand _addChildGlobalVariableGroupCommand;
        public static RoutedCommand AddChildGlobalVariableGroup
        {
            get
            {
                if (_addChildGlobalVariableGroupCommand == null)
                    _addChildGlobalVariableGroupCommand = new RoutedCommand();
                return _addChildGlobalVariableGroupCommand;
            }
        }

        static RoutedCommand _addGlobalVariableCommand;
        public static RoutedCommand AddGlobalVariable
        {
            get
            {
                if (_addGlobalVariableCommand == null)
                    _addGlobalVariableCommand = new RoutedCommand();
                return _addGlobalVariableCommand;
            }
        }

        static RoutedCommand _addGlobalTableVariableCommand;
        public static RoutedCommand AddGlobalTableVariable
        {
            get
            {
                if (_addGlobalTableVariableCommand == null)
                    _addGlobalTableVariableCommand = new RoutedCommand();
                return _addGlobalTableVariableCommand;
            }
        }

        #endregion
    }
}
