import {combineReducers} from 'redux';
import { reducer as authenticationReducer } from './authentication/slice';
import {loadState, saveState} from "./localstorage";
import { throttle } from 'lodash';
import {configureStore} from "@reduxjs/toolkit";

const rootReducer = combineReducers({
    user: authenticationReducer
});

const loadedStateFromLocalStorage = loadState();

export const store = configureStore(
    {
        reducer: rootReducer,
        preloadedState: loadedStateFromLocalStorage,
    });

store.subscribe(
    throttle(
        () => {
            saveState(store.getState());
        }, 1000
    )
);