import React from "react";
import { Button, Col, FormControl, InputGroup, ListGroup, Row } from "react-bootstrap";
import SearchIcon from "../assets/search-icon.svg";

export interface ISearchBoxProps {
  placeholder?: string;
  searchResults: Map<string, string>; //key - id, value - search result item
  onChange: (keyword: string) => void;
  onActionButtonClick: (selectedItemId: string) => void;
  actionButtonText: string;
  onHide?: () => void;
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

  handleInputChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const { onChange } = this.props;

    this.setState({ inputValue: event.target.value });
    onChange(event.target.value);
  };

  handleSearchResultClick = (selectedItemId: string): void => {
    const { onActionButtonClick } = this.props;
    this.setState({ inputValue: "" });
    onActionButtonClick(selectedItemId);
  };

  renderSearchResultsTable = (): JSX.Element => {
    const { searchResults, actionButtonText } = this.props;

    return (
      <ListGroup className="shadow-sm">
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
    const { placeholder, onHide } = this.props;
    const { inputValue } = this.state;

    return (
      <div>
        <InputGroup>
          <FormControl
            placeholder={placeholder}
            value={inputValue}
            onChange={this.handleInputChange}
          />
          <div className="input-group-append">
            <span className="input-group-text">
              <img src={SearchIcon} height="24px" width="24px" alt="Search Icon" />
            </span>
          </div>
        </InputGroup>
        {inputValue && this.renderSearchResultsTable()}
      </div>
    );
  }
}