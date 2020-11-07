/* eslint-disable react/destructuring-assignment */
/* eslint-disable react/jsx-props-no-spreading */

import React from "react";
import { Route, Redirect, RouteProps } from "react-router-dom";
import UserContext from "./backend/helpers/UserContext";

export interface PrivateRouteProps extends RouteProps {
  children: JSX.Element;
}

// eslint-disable-next-line @typescript-eslint/naming-convention
export function PrivateRoute({ children, ...rest }: RouteProps): JSX.Element {
  return (
    <Route
      {...rest}
      render={(props): React.ReactNode => {
        const currentUser = UserContext.user;
        if (!currentUser) {
          // not logged in so redirect to login page with the return url
          return (
            <Redirect
              to={{ pathname: "/login", state: { from: props.location } }}
            />
          );
        }

        // authorised so return component
        return children;
      }}
    />
  );
}
