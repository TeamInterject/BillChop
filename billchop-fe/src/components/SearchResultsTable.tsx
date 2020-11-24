import * as React from "react";
import { ListGroup, Row, Col, Button } from "react-bootstrap";

export interface ISearchResultsTableProps { 
  searchResults: Map<string, string>, 
  actionButtonText: string 
  handleSearchResultClick: (selectedItemId: string) => void,
}

export default class SearchResultsTable extends React.Component<ISearchResultsTableProps> {
  constructor(props: ISearchResultsTableProps) {
    super(props);
    this.state = {};
  }

  public render(): JSX.Element {
    const { searchResults, actionButtonText, handleSearchResultClick } = this.props;

    const tableContent = () => {
      if(searchResults.size === 0)
        return (
          <ListGroup.Item>
            No search results were found.
          </ListGroup.Item>
        );
  
      return Array
        .from(searchResults)
        .map(([key, value]) => (
          <SearchResultsRow 
            key={key}
            id={key}
            value={value}
            actionButtonText={actionButtonText}
            onActionButtonClick={handleSearchResultClick}
          />));
    };
  
    return (
      <ListGroup className="shadow-sm search-box__search-results">
        {tableContent()}
      </ListGroup>
    );
  }
}

function SearchResultsRow(props: {
  id: string,
  value: string,
  actionButtonText: string,
  onActionButtonClick: (selectedItemId: string) => void,
}): JSX.Element {
  const {id, value, actionButtonText, onActionButtonClick} = props;

  function handleSearchResultClick() {
    onActionButtonClick(id);
  }

  return (
    <ListGroup.Item
      className="py-2 search-box__search-results-item"
      onClick={handleSearchResultClick}
    >
      <Row>
        <Col className="d-flex align-items-center">
          {value}
        </Col>
        <Col className="col-1">
          <Button
            variant="light"
            onClick={handleSearchResultClick}
          >
            {actionButtonText}
          </Button>
        </Col>
      </Row>
    </ListGroup.Item>
  );
}