/* eslint-disable react/destructuring-assignment */
/* eslint-disable react/jsx-props-no-spreading */

import React from "react";
import { Route, Redirect, RouteProps } from "react-router-dom";
import UserContext from "./backend/helpers/UserContext";

export interface PrivateRouteProps extends RouteProps {
  children: JSX.Element;
}

// eslint-disable-next-line @typescript-eslint/naming-convention
export function NonPrivateRoute({ children, ...rest }: RouteProps): JSX.Element {
  return (
    <Route
      {...rest}
      render={(props): React.ReactNode => {
        const currentUser = UserContext.user;
        if (currentUser) {
          return (
            <Redirect
              to={{ pathname: "/", state: { from: props.location } }}
            />
          );
        }

        return children;
      }}
    />
  );
}
