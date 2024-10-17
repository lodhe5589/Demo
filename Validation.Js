function mainFunction(){

  if(!validation())
    return false;
  
  alert('Done');
}

function validation(){
  
  const Number = 300;
  const Name = 'Rahul';
  const age = 25;

  if(Number != 200)
    return false;

  return true;
}
