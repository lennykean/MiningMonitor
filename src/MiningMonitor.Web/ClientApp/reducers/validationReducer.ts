import { Reducer } from 'redux';

import { Action, ActionType } from '../actions';
import { ValidationState } from '../store';

const initialState = {
    validation: {},
};

export const validationReducer: Reducer<ValidationState> = (state: ValidationState = initialState, action: Action) => {
    switch (action.type) {
        case ActionType.Validation:
            return {
                validation: action.validation,
            };

        default:
            return state;
    }
};
