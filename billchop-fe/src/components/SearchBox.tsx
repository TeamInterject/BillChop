import React from "react";
import { debounce } from "lodash";
import { Button, FormControl, InputGroup } from "react-bootstrap";
import SearchIcon from "../assets/search-icon.svg";
import ArrowBackIcon from "../assets/arrow-back-icon.svg";
import OutsideClickListener from "./OutsideClickListener";
import "../styles/search-box.css";
import SearchResultsTable from "./SearchResultsTable";

export interface ISearchBoxProps {
  placeholder?: string;
  searchResults: Map<string, string>; //key - id, value - search result item
  onChange: (keyword: string) => void;
  onActionButtonClick: (selectedItemId: string) => void;
  actionButtonText: string;
  onHide: () => void;
}

interface ISearchBoxState {
  inputValue: string;
}

export default class SearchBox extends React.Component<
  ISearchBoxProps,
  ISearchBoxState
  > {
  constructor(props: ISearchBoxProps) {
    super(props);

    this.state = {
      inputValue: "",
    };
  }

  handleInputChange = (inputValue: string): void => {
    if (!inputValue) return;

    const { onChange } = this.props;
    onChange(inputValue);
  };

  debouncedHandleInputChange = debounce(this.handleInputChange, 300);

  delayedHandleInputChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const inputValue = event.target.value;
    this.setState({ inputValue });

    this.debouncedHandleInputChange(inputValue);
  };

  handleSearchResultClick = (selectedItemId: string): void => {
    const { onActionButtonClick } = this.props;
    this.setState({ inputValue: "" });
    onActionButtonClick(selectedItemId);
  };

  render(): JSX.Element {
    const { searchResults, actionButtonText, placeholder, onHide, onChange } = this.props;
    const { inputValue } = this.state;

    return (
      <OutsideClickListener onOutsideClick={onHide}>
        <div className="search-box">
          <InputGroup>
            <InputGroup.Prepend>
              <Button variant="outline-secondary" size="sm" onClick={onHide}>
                <img src={ArrowBackIcon} height="24px" width="24px" alt="Go back" />
              </Button>
            </InputGroup.Prepend>
            <FormControl
              placeholder={placeholder}
              value={inputValue}
              onChange={this.delayedHandleInputChange}
            />
            <InputGroup.Append>
              <Button variant="outline-secondary" size="sm" onClick={() => onChange(inputValue)}>
                <img src={SearchIcon} height="24px" width="24px" alt="Search Icon" />
              </Button>
            </InputGroup.Append>
          </InputGroup>
          {inputValue && <SearchResultsTable 
            searchResults={searchResults}
            actionButtonText={actionButtonText}
            handleSearchResultClick={this.handleSearchResultClick}
          />}
        </div>
      </OutsideClickListener>
    );
  }
}