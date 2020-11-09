/* eslint-disable react/prop-types */
import React, { ReactNode, useCallback, useEffect, useRef } from "react";

export interface IOutsideClickListenerProps {
  onOutsideClick: () => void;
  children?: ReactNode;
}

const OutsideClickListener: React.FunctionComponent<IOutsideClickListenerProps> = (props) => {
  const ref = useRef<HTMLDivElement>(null);

  const { onOutsideClick } = props;

  const escapeListener = useCallback((e: KeyboardEvent) => {
    if (e.key === "Escape") {
      onOutsideClick();
    }
  }, [onOutsideClick]);

  const clickListener = useCallback(
    (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        onOutsideClick();
      }
    },
    [onOutsideClick],
  );

  useEffect(() => {

    document.addEventListener("click", clickListener);
    document.addEventListener("keyup", escapeListener);

    return () => {
      document.removeEventListener("click", clickListener);
      document.removeEventListener("keyup", escapeListener);
    };
  }, [clickListener, escapeListener]);

  return (
    <div ref={ref}>
      {props.children}
    </div>
  );
};

export default OutsideClickListener;