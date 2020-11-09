import React from "react";
import { debounce } from "lodash";
import { Button, Col, FormControl, InputGroup, ListGroup, Row } from "react-bootstrap";
import SearchIcon from "../assets/search-icon.svg";
import ArrowBackIcon from "../assets/arrow-back-icon.svg";
import OutsideClickListener from "./OutsideClickListener";
import "../styles/search-box.css";

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

  delayedHandleInputChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const inputValue = event.target.value;
    this.setState({ inputValue });
    const delayed = debounce(() => this.handleInputChange(inputValue), 300);
    delayed();
  };

  handleSearchResultClick = (selectedItemId: string): void => {
    const { onActionButtonClick } = this.props;
    this.setState({ inputValue: "" });
    onActionButtonClick(selectedItemId);
  };

  renderSearchResultsTable = (): JSX.Element => {
    const { searchResults, actionButtonText } = this.props;

    return (
      <ListGroup className="shadow-sm search-box__search-results">
        {
          searchResults.size !== 0 ?
            Array.from(searchResults).map(([key, value]) => {
              return (
                <ListGroup.Item
                  key={key}
                  className="py-2"
                  onClick={() => this.handleSearchResultClick(key)}
                  style={{ cursor: "pointer" }}
                >
                  <Row>
                    <Col className="d-flex align-items-center">
                      {value}
                    </Col>
                    <Col className="col-1">
                      <Button
                        variant="light"
                        onClick={() => this.handleSearchResultClick(key)}
                      >
                        {actionButtonText}
                      </Button>
                    </Col>
                  </Row>
                </ListGroup.Item>
              );
            })
            :
            <ListGroup.Item>
              No search results were found.
            </ListGroup.Item>
        }
      </ListGroup>
    );
  };

  render(): JSX.Element {
    const { placeholder, onHide, onChange } = this.props;
    const { inputValue } = this.state;

    return (
      <OutsideClickListener onClickOutside={onHide}>
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
          {inputValue && this.renderSearchResultsTable()}
        </div>
      </OutsideClickListener>
    );
  }
}