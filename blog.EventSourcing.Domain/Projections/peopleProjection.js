fromCategory('Person')
  .whenAny(function (s, e) {
      linkTo('people', e);
  });