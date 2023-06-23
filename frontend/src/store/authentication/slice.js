import {userConstants} from "../../constants/user.constants";
const { createSlice } = require("@reduxjs/toolkit");

let user = JSON.parse(localStorage.getItem('user'));

const authenticationSlice = createSlice({ 
    name: 'user',
    initialState: user ? { loggedIn: true, user } : {},
    reducers: {
        authentication(state, action) {
            switch (action.type) {
                case userConstants.LOGIN_REQUEST:
                    return {
                        loggingIn: true,
                        user: action.user
                    };
                case userConstants.LOGIN_SUCCESS:
                    return {
                        loggedIn: true,
                        user: action.user
                    };
                case userConstants.LOGIN_FAILURE:
                    return {};
                case userConstants.LOGOUT:
                    return {};
                default:
                    return state
            }
        }
    }
});

export const {actions, reducer} = authenticationSlice;
export const { authentication } = actions;