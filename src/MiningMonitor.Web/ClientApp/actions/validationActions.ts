import { ThunkAction } from 'redux-thunk';

import { Action, ActionType } from '.';
import { AppState } from '../store';

export type ClearValidationErrorsAction = () => ThunkAction<void, AppState, any>;

interface ValidationActions {
    clearValidationErrors: ClearValidationErrorsAction;
}

export const validationActions: ValidationActions = {
    clearValidationErrors() {
        return (dispatch, getState) => {
            dispatch<Action>({
                type: ActionType.Validation,
                validation: {},
            });
        };
    },
};
